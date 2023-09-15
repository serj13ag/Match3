using System;

namespace Data
{
    [Serializable]
    public class PlayerData
    {
        public PlayerProgress PlayerProgress { get; set; }
        public GameSettings GameSettings { get; set; }

        public PlayerData()
        {
            PlayerProgress = new PlayerProgress();
            GameSettings = new GameSettings();
        }
    }
}