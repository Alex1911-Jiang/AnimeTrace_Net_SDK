using Newtonsoft.Json;

namespace AnimeTrace_Net_SDK
{
    public class AnimeTraceCharacterItem
    {
        [JsonProperty("work")]
        public string Work { get; set; }

        [JsonProperty("character")]
        public string Character { get; set; }
    }
}
