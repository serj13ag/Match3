using System;
using Enums;
using UnityEngine;

namespace PersistentData
{
    [Serializable]
    public class ColorDataEntry
    {
        public GamePieceColor Type;
        public Color Color;
    }
}