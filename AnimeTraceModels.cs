using System;

namespace AnimeTrace_Net_SDK
{
    public class AnimeTraceModels
    {
        private string _modelName;

        public static implicit operator string(AnimeTraceModels self) => self._modelName;
        public override string ToString() => _modelName;

        private AnimeTraceModels(string modelName) => _modelName = modelName;

        public static AnimeTraceModels Anime = new("anime");
        public static AnimeTraceModels AnimeModelLovelive { get; } = new AnimeTraceModels("anime_model_lovelive");
        public static AnimeTraceModels PreStable { get; } = new AnimeTraceModels("pre_stable");
        public static AnimeTraceModels FullGameModelKira { get; } = new AnimeTraceModels("full_game_model_kira");

        [Obsolete]
        public static AnimeTraceModels Game { get; } = new AnimeTraceModels("game");
        [Obsolete]
        public static AnimeTraceModels GameModelKirakira { get; } = new AnimeTraceModels("game_model_kirakira");
    }
}
