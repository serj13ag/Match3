using System;

namespace Data
{
    [Serializable]
    public class BoardData
    {
        public LevelBoardData LevelBoardData;

        public BoardData()
        {
            LevelBoardData = new LevelBoardData();
        }
    }
}