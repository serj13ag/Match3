using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class PlayerProgress
    {
        public int PlayerLevel { get; set; }
        public int Coins { get; set; }
        public Dictionary<string, LevelBoardData> BoardData { get; set; }

        public PlayerProgress()
        {
            PlayerLevel = 1;
            BoardData = new Dictionary<string, LevelBoardData>();
        }
    }
}