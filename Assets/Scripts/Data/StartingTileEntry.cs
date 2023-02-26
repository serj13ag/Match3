using System;
using Entities;

namespace Data
{
    [Serializable]
    public class StartingTileEntry
    {
        public Tile TilePrefab;
        public int X;
        public int Y;
        public int Z;
    }
}