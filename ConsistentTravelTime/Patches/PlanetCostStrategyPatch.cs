using BattleTech;
using BattleTech.UI;
using Harmony;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsistentTravelTime.Patches
{
    [HarmonyPatch(typeof(SGTravelManager), nameof(SGTravelManager.HandleNextTravelStep))]
    public static class ChangeInSystemTransitTime
    {

        public static bool Prepare()
        {
            return (Core.ModSettings.PlanetTravelStrategy == PlanetTravelStrategy.PlanetCost);
        }

        public static void Prefix(SGTravelManager __instance, SimGameState ___simState)
        {
            if (Core.ModSettings.Debug) Logger.Log($"ChangeInSystemTransitTime Travel State {__instance.TravelState}");
            if (Core.ModSettings.Debug) Logger.Log($"ChangeInSystemTransitTime Travel Time {___simState.TravelTime}");
            


        }


        public static void Postfix(SGTravelManager __instance, SimGameState ___simState)
        {
            //Note - This Patch is the original mod code for the "Planet cost" navigation cost.  The JumpDistance cost is in a different patch
            //  In the future, this code could probably be moved to the other function, but keeping as is since the code has been tested to work
            try
            {
                if (Core.ModSettings.Debug)
                {
                    Logger.Log("ChangeInSystemTransitTime ---- Start");
                    Logger.Log($"ChangeInSystemTransitTime Planet: {___simState.CurSystem.Name}");
                    Logger.Log($"ChangeInSystemTransitTime Planet: Date - {___simState.DaysPassed}");

                    Logger.LogJson(new
                    {   
                        ___simState.CurSystem.Name,
                        ___simState.DaysPassed,
                        Pre = __instance.PreTransitionState.ToString(),
                        post = __instance.PostTransitionState.ToString(),
                        Travel = __instance.TravelState.ToString(),
                        ExistingTravelTime = ___simState.TravelTime,

                    });

                    StarSystemNode starSystemNode = ___simState.Starmap.GetSystemByID(___simState.CurSystem.ID);

                    Logger.LogJson(new
                    {
                        NodeCost = starSystemNode.Cost,
                        jumpDistance = ___simState.CurSystem.JumpDistance,
                        planetCost = ___simState.Starmap.CurPlanet.Cost,
                    });

                }


                int travelTime;

                if (Core.ModSettings.PlanetTravelStrategy == PlanetTravelStrategy.PlanetCost 
                    && __instance.PreTransitionState == SimGameTravelStatus.WARMING_ENGINES 
                    && __instance.PostTransitionState == SimGameTravelStatus.TRANSIT_FROM_JUMP)
                {
                    //Navigation ended 
                    if (Core.ModSettings.Debug) Logger.Log("ChangeInSystemTransitTime Navigation ended mode");

                    int jumpDistance = ___simState.CurSystem.JumpDistance;
                    int planetCost = ___simState.Starmap.CurPlanet.Cost;

                    if (Core.ModSettings.UseLowerAmount)
                    {
                        travelTime = jumpDistance < planetCost ? jumpDistance : planetCost;
                    }
                    else
                    {
                        travelTime = planetCost;
                    }

                }
                else
                {
                    //no matching mode.
                    if (Core.ModSettings.Debug) Logger.Log("ChangeInSystemTransitTime ---- End (No Match)");

                    return;
                }
                
                ___simState.SetTravelTime(travelTime, null);

                if (Core.ModSettings.Debug) Logger.Log($"ChangeInSystemTransitTime: Travel Time {travelTime}");
                if (Core.ModSettings.Debug) Logger.Log("ChangeInSystemTransitTime ---- End");

            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

    }
}
