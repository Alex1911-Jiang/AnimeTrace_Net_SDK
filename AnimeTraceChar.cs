using Newtonsoft.Json;

namespace AnimeTrace_Net_SDK
{
    public class AnimeTraceChar
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cartoonname")]
        public string CartoonName { get; set; }

        [JsonProperty("acc")]
        public float Acc { get; set; }
    }
}
