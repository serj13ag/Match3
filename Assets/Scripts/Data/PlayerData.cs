using System;

namespace Data
{
    [Serializable]
    public class PlayerData
    {
        public PlayerProgress PlayerProgress { get; set; }
        public Purchases Purchases { get; set; }
        public Customizations Customizations { get; set; }
        public GameSettings GameSettings { get; set; }

        public PlayerData()
        {
            PlayerProgress = new PlayerProgress();
            Purchases = new Purchases();
            Customizations = new Customizations();
            GameSettings = new GameSettings();
        }
    }
}