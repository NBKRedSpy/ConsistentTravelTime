using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsistentTravelTime
{
    public class ModSettings
    {
        /// <summary>
        /// If true will output debugger messages to the Log.txt file.
        /// </summary>
        public bool Debug { get; set; }

        public PlanetTravelStrategy PlanetTravelStrategy { get; set; } = PlanetTravelStrategy.PlanetCost;

        /// <summary>
        /// Used if PlanetTravelStrategy is set to PlanetCost.
        /// If true, will use the lower of "planet cost" (3 days) or the "Jump distance" (1 - 16 days).
        /// Otherwise will the "planet cost"
        /// </summary>
        public bool UseLowerAmount { get; set; } = true;
    }
}
