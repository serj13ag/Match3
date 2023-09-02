using System;

namespace Data
{
    [Serializable]
    public class GameSettings
    {
        public bool MusicEnabled { get; set; }
        public bool SoundEnabled { get; set; }

        public GameSettings()
        {
            MusicEnabled = true;
            SoundEnabled = true;
        }
    }
}