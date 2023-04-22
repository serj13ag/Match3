using System;
using Enums;

namespace PersistentData
{
    [Serializable]
    public class StartingTileEntry : StartingObjectEntry
    {
        public TileType TileType;
    }
}