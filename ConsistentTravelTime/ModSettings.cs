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

        public PlanetTravelStrategy PlanetTravelStrategy { get; set; } = PlanetTravelStrategy.JumpDistance;
    }
}
