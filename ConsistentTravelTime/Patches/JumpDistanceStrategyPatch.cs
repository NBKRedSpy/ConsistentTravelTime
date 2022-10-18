using BattleTech;
using BattleTech.Save.SaveGameStructure;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ConsistentTravelTime.Patches
{
    [HarmonyPatch(typeof(Starmap), "CreateActivePath")]
    internal class CancelNavigationTimePatch
    {

        public static bool Prepare()
        {
            return (Core.ModSettings.PlanetTravelStrategy == PlanetTravelStrategy.JumpDistance);
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {

            try
            {

                List<Label> switchLables = new List<Label>();


                List<CodeInstruction> instructionList = new List<CodeInstruction>(instructions);

                //---get the switch label
                CodeInstruction switchOp = instructionList.First(x => x.opcode == OpCodes.Switch);

                Label[] switchLabels = switchOp.operand as Label[];

                Label warmingEnginesLabel = switchLabels[2]; //The "case SimGameTravelStatus.WARMING_ENGINES:" code.

                //--find the switch jump location
                int warmingEnginesLabelIndex = instructionList.FindIndex(x => x.labels.Contains(warmingEnginesLabel));

                if (warmingEnginesLabelIndex == -1)
                {
                    throw new ApplicationException("Could not find warming engines label");
                }

                //Expect the following after the label:
                // skip:
                //  L_0087: Label2
                //  L_0087: Label3
                //  L_0087: ldc.i4.2
                //  L_0088: stloc.0
                //  L_0089: ldarg.0

                int currentIndex = warmingEnginesLabelIndex;

                if (
                    (instructionList[currentIndex].opcode != OpCodes.Ldc_I4_2) ||
                    (instructionList[currentIndex + 1].opcode != OpCodes.Stloc_0) ||
                    (instructionList[currentIndex + 2].opcode != OpCodes.Ldarg_0) ||
                    (instructionList[currentIndex + 3].opcode != OpCodes.Call) ||
                    (instructionList[currentIndex + 4].opcode != OpCodes.Callvirt)
                    )
                {
                    throw new ApplicationException("Did not find Warming Engine pre code");
                }

                //todo:  Finish here - find a way to verify the call targets to make sure the expected code is in place.

                currentIndex += 3;

                //Remove:
                //  L_008a: call BattleTech.StarSystemNode get_CurPlanet()
                //  L_008f: callvirt Int32 get_Cost()
                instructionList.RemoveRange(currentIndex, 2);

                List<CodeInstruction> newCode = GetNewCode();
                instructionList.InsertRange(currentIndex, newCode);

                currentIndex += newCode.Count;


                //should follow:
                //  L_0081: stloc.1
                //  L_0082: br Label7

                //verify follow up code
                if (
                    (instructionList[currentIndex].opcode != OpCodes.Stloc_1) ||
                    (instructionList[currentIndex + 1].opcode != OpCodes.Br))
                {
                    throw new ApplicationException("Did not find Warming Engine post code");
                }

                return instructionList;

            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw;
            }
        }

        private static List<CodeInstruction> GetNewCode()
        {

            List<CodeInstruction> newInstructions = new List<CodeInstruction>();

            CodeInstruction instruction;

            //Goal:
            //  L_0072: ldfld BattleTech.SimGameState sim
            //  L_0077: callvirt BattleTech.StarSystem get_CurSystem()
            //  L_007c: callvirt Int32 get_JumpDistance()


            //  L_0072: ldfld BattleTech.SimGameState sim
            var simGameStateField = AccessTools.Field(typeof(Starmap), "sim");
            newInstructions.Add(new CodeInstruction(OpCodes.Ldfld, simGameStateField));

            //  L_0077: callvirt BattleTech.StarSystem get_CurSystem()
            var currentSystemPropertyGetter = AccessTools.Property(typeof(SimGameState), nameof(SimGameState.CurSystem)).GetGetMethod();
            newInstructions.Add(new CodeInstruction(OpCodes.Callvirt, currentSystemPropertyGetter));

            //  L_007c: callvirt Int32 get_JumpDistance()
            var starSystemJumpDistanceGetter = AccessTools.Property(typeof(StarSystem), nameof(StarSystem.JumpDistance)).GetGetMethod();
            newInstructions.Add(new CodeInstruction(OpCodes.Callvirt, starSystemJumpDistanceGetter));

            return newInstructions;
        }


    }
}
