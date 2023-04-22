using System;
using Entities;

namespace PersistentData
{
    [Serializable]
    public class StartingTileEntry : StartingObjectEntry
    {
        public Tile TilePrefab;
    }
}