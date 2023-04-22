using System;
using Entities.Tiles;
using Enums;

namespace PersistentData
{
    [Serializable]
    public class TileModel
    {
        public TileType Type;
        public BaseTile TilePrefab;
        public int MatchesTillBreak;
        public BreakableSpriteData[] BreakableSpriteData;
    }
}