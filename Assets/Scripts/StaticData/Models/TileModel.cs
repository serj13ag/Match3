using System;
using Entities.Tiles;
using Enums;

namespace StaticData.Models
{
    [Serializable]
    public class TileModel
    {
        public TileType Type;
        public BaseTile TilePrefab;
        public int MatchesTillBreak;
        public BreakableSpriteModel[] BreakableSpriteData;
    }
}