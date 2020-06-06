using System;
using Newtonsoft.Json;

namespace WoWToken.Server.Data.Models.Battlenet
{
    public class WoWToken
    {
        [JsonProperty("last_updated_timestamp")]
        public long LastUpdatedTimestamp { get; set; }

        [JsonProperty("price")]
        public long Price { get; set; }
    }
}
