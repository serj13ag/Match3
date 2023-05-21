using System;
using Interfaces;

namespace Data
{
    [Serializable]
    public class LevelBoardData
    {
        public string LevelName;
        public ITile[,] Tiles;

        public LevelBoardData(string levelName)
        {
            LevelName = levelName;
        }

        public LevelBoardData(string levelName, ITile[,] tiles)
        {
            LevelName = levelName;
            Tiles = tiles;
        }
    }
}