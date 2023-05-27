﻿using StaticData.Models;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "ColorData", menuName = "Create Color Data")]
    public class ColorData : ScriptableObject
    {
        public ColorDataModel[] GamePieceColors;
    }
}