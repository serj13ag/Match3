using System;
using Enums;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "ColorsData", menuName = "StaticData/Colors")]
    public class ColorsStaticData : ScriptableObject
    {
        public ColorStaticData[] GamePieceColors;
    }

    [Serializable]
    public class ColorStaticData
    {
        public GamePieceColor Type;
        public Color Color;
    }
}