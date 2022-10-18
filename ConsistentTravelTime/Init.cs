using Harmony;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsistentTravelTime
{
    public static class HarmonyInit
    {
        public static void Init(string directory, string settingsJSON)
        {
            Core.ModSettings = Newtonsoft.Json.JsonConvert.DeserializeObject<ModSettings>(settingsJSON);


            if(Enum.IsDefined(typeof(PlanetTravelStrategy), Core.ModSettings.PlanetTravelStrategy))
            {
                if (Core.ModSettings.Debug) Logger.Log($"PlanetTravelStrategy {Core.ModSettings.PlanetTravelStrategy}");

                var harmony = HarmonyInstance.Create("io.github.nbk_redspy.ConsistentTravelTime");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            else
            {
                if (Core.ModSettings.Debug) Logger.Log($"Init: PlanetTravelStrategy is invalid:  ${Core.ModSettings.PlanetTravelStrategy}");
            }
        }
    }
}
