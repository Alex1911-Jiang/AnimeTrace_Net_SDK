using System;
using Newtonsoft.Json;

namespace AnimeTrace_Net_SDK
{
    public class AnimeTraceDatum
    {
        [JsonProperty("box")]
        public float[] Box { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cartoonname")]
        public string CartoonName { get; set; }

        [JsonProperty("acc_percent")]
        public float AccPercent { get; set; }

        [JsonProperty("char")]
        public AnimeTraceChar[] Char { get; set; }

        [JsonProperty("box_id")]
        public string BoxId { get; set; }
    }
}
