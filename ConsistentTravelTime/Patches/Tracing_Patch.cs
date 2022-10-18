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

    [HarmonyPatch(typeof(Starmap), nameof(Starmap.CancelTravelAndMoveToCurrentSystem))]
    public static class Starmap_CancelTravelAndMoveToCurrentSystem
    {
        public static bool Prepare() 
        {
            return false;
        }


        public static void Prefix(Starmap __instance, SimGameState ___sim)
        {
            if (Core.ModSettings.Debug)
            {
                Logger.LogJson(new
                {
                    message = $"CancelTravelAndMoveToCurrentSystem hit",
                    __instance.ProjectedTravelTime,
                    ___sim.CurSystem.JumpDistance,
                });
            }


        }
    }


    [HarmonyPatch(typeof(Starmap), "CreateActivePath")]
    public static class CreateActivePath_Patch
    {
        public static void Prefix(Starmap __instance, SimGameState ___sim)
        {
            if (Core.ModSettings.Debug)
            {
                Logger.LogJson(new
                {
                    Message = "CreateActivePath",
                    TravelState = ___sim.TravelState.ToString(),
                    ___sim.CurSystem.JumpDistance,
                    __instance.CurPlanet.Cost
                });
            }


        }
    }

    [HarmonyPatch(typeof(SGTravelManager), nameof(SGTravelManager.SetTravelState))]
    public static class SGTravelManager_SetTravelState
    {
        public static void Prefix(SGTravelManager __instance, SimGameState ___simState, SimGameTravelStatus newStatus)
        {
            if (Core.ModSettings.Debug)
            {
                Logger.LogJson(new
                {
                    Message = "SGTravelManager.SetTravelState - Prefix",
                    newStatus = newStatus.ToString(),
                    SGTravelManager_TravelState = __instance.TravelState.ToString()
                });
            }
        }

        public static void Postfix(SGTravelManager __instance, SimGameState ___simState)
        {
            if (Core.ModSettings.Debug)
            {
                Logger.LogJson(new
                {
                    Message = "SGTravelManager.SetTravelState - Post fix",
                    SGTravelManager_TravelState = __instance.TravelState.ToString(),
                    PreTransitionState = __instance.PreTransitionState.ToString(),
                    PostTransitionState = __instance.PostTransitionState.ToString(),
                });
            }
        }
    }
}
