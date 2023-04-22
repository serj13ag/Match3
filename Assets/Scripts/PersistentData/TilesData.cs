using System;
using Entities;
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
        public BasicTile TilePrefab;
        public int MatchesTillBreak;
        public BreakableSpriteData[] BreakableSpriteData;
    }
}