using System;

namespace Data
{
    [Serializable]
    public class GameSettings
    {
        public bool SoundEnabled { get; set; }

        public GameSettings()
        {
            SoundEnabled = true;
        }
    }
}