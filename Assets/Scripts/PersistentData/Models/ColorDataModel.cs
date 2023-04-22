using System;
using Enums;
using UnityEngine;

namespace PersistentData.Models
{
    [Serializable]
    public class ColorDataModel
    {
        public GamePieceColor Type;
        public Color Color;
    }
}