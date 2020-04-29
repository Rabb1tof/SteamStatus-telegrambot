using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SteamStatusBot.SteamStats
{
    public class Json
    {
        [JsonProperty("time")] public int Time { get; set; }
        [JsonProperty("online")] public double Online { get; set; }

        /// <summary>
        /// @param - id (csgo, store, etc...)
        /// @param - bool (die?)
        /// @param - status (normal, ok, etc...)
        /// </summary>
        [JsonProperty("services")]
        public List<List<object>> Services { get; set; }
    }
}
