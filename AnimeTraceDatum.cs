using Newtonsoft.Json;

namespace AnimeTrace_Net_SDK
{
    public class AnimeTraceDatum
    {
        [JsonProperty("box")]
        public float[] Box { get; set; }

        [JsonProperty("box_id")]
        public string BoxId { get; set; }

        [JsonProperty("character")]
        public AnimeTraceCharacterItem[] Character { get; set; }
    }
}
