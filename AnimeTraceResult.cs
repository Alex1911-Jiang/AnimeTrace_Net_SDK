using Newtonsoft.Json;

namespace AnimeTrace_Net_SDK
{
    public class AnimeTraceResult
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public AnimeTraceDatum[] Data { get; set; }

        [JsonProperty("ai")]
        public bool Ai { get; set; }

        [JsonProperty("new_code")]
        public int NewCode { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }
    }
}
