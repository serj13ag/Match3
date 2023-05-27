using System;

namespace Data
{
    [Serializable]
    public class BoardData
    {
        public LevelBoardData LevelBoardData;

        public BoardData(string levelName)
        {
            LevelBoardData = new LevelBoardData(levelName);
        }
    }
}