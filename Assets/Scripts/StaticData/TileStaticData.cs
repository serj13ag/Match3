using System;
using Entities.Tiles;
using Enums;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "TileData", menuName = "StaticData/Tile")]
    public class TileStaticData : ScriptableObject
    {
        public TileType Type;
        public BaseTile Prefab;
        public int MatchesTillBreak;
        public BreakableSpriteStaticData[] BreakableSprites;
    }

    [Serializable]
    public class BreakableSpriteStaticData
    {
        public Sprite Sprite;
        public Color Color;
    }
}