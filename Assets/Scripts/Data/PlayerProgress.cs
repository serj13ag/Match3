using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class PlayerProgress
    {
        public int PlayerLevel;
        public Dictionary<string, LevelBoardData> BoardData;

        public PlayerProgress()
        {
            PlayerLevel = 1;
            BoardData = new Dictionary<string, LevelBoardData>();
        }
    }
}