using System;
using Entities;

namespace Data
{
    [Serializable]
    public class StartingTileEntry : StartingObjectEntry
    {
        public Tile TilePrefab;
    }
}