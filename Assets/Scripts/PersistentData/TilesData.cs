using System;
using Entities.Tiles;
using Enums;
using UnityEngine;

namespace PersistentData
{
    [CreateAssetMenu(fileName = "TilesData", menuName = "Create Tiles")]
    public class TilesData : ScriptableObject
    {
        public TileModel[] Tiles;
    }

    [Serializable]
    public class TileModel
    {
        public TileType Type;
        public BaseTile TilePrefab;
        public int MatchesTillBreak;
        public BreakableSpriteData[] BreakableSpriteData;
    }
}