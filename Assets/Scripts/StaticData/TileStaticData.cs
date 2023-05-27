using System;
using Entities.Tiles;
using Enums;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "TileStaticData", menuName = "StaticData/Tile")]
    public class TileStaticData : ScriptableObject
    {
        public TileType Type;
        public BaseTile Prefab;
        public int MatchesTillBreak;
        public BreakableSpriteStaticData[] BreakableSpriteData;
    }

    [Serializable]
    public class BreakableSpriteStaticData
    {
        public Sprite BreakableSprite;
        public Color BreakableColor;
    }
}