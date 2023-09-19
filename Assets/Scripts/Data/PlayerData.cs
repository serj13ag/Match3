using System;

namespace Data
{
    [Serializable]
    public class PlayerData
    {
        public PlayerProgress PlayerProgress { get; set; }
        public Purchases Purchases { get; set; }
        public GameSettings GameSettings { get; set; }

        public PlayerData()
        {
            PlayerProgress = new PlayerProgress();
            Purchases = new Purchases();
            GameSettings = new GameSettings();
        }
    }
}