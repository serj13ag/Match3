using System;

namespace Data
{
    [Serializable]
    public class GameSettings
    {
        public bool SoundEnabled;

        public GameSettings()
        {
            SoundEnabled = true;
        }
    }
}