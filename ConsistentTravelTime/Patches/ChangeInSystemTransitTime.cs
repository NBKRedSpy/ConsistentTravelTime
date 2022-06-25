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

        public static void Prefix(SGTravelManager __instance, SimGameState ___simState)
        {
            if (Core.ModSettings.Debug) Logger.Log($"ChangeInSystemTransitTime Travel State {__instance.TravelState}");

        }


        public static void Postfix(SGTravelManager __instance, SimGameState ___simState)
        {
            try
            {
                if(Core.ModSettings.Debug) Logger.Log("ChangeInSystemTransitTime Start");

                if (Core.ModSettings.Debug) Logger.LogJson(new
                {
                    Pre = __instance.PreTransitionState.ToString(),
                    post = __instance.PostTransitionState.ToString(),
                    Travel = __instance.TravelState.ToString()
                });


                if (Core.ModSettings.Debug) Logger.Log($"ChangeInSystemTransitTime Preexisting Travel Time: {___simState.TravelTime}");


                if (
                    !(__instance.PreTransitionState == SimGameTravelStatus.WARMING_ENGINES && 
                        __instance.PostTransitionState == SimGameTravelStatus.TRANSIT_FROM_JUMP))
                {
                    return;
                }


                //Navigation ended 
                if (Core.ModSettings.Debug) if (Core.ModSettings.Debug) Logger.Log("ChangeInSystemTransitTime Entered");

                int jumpDistance = ___simState.CurSystem.JumpDistance;
                int planetCost = ___simState.Starmap.CurPlanet.Cost;

                int travelTime;

                switch (Core.ModSettings.PlanetTravelStrategy)
                {
                    case PlanetTravelStrategy.PlanetCost:
                        if(Core.ModSettings.UseLowerAmount)
                        {
                            travelTime = jumpDistance < planetCost ? jumpDistance : planetCost;
                        }
                        else
                        {
                            travelTime = planetCost;
                        }
                        break;
                    case PlanetTravelStrategy.JumpDistance:
                        travelTime = jumpDistance;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("PlanetTravelStrategy");
                }

                ___simState.SetTravelTime(travelTime, null);

                if (Core.ModSettings.Debug) Logger.Log($"ChangeInSystemTransitTime: Travel Time {travelTime}");
                if (Core.ModSettings.Debug) Logger.Log($"ChangeInSystemTransitTime end");


            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

    }
}
