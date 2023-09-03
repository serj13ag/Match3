using System;
using Enums;

namespace Data
{
    [Serializable]
    public class GameSettings
    {
        public bool MusicEnabled { get; set; }
        public bool SoundEnabled { get; set; }
        public LanguageType Language { get; set; }

        public GameSettings()
        {
            MusicEnabled = true;
            SoundEnabled = true;
            Language = LanguageType.English;
        }
    }
}