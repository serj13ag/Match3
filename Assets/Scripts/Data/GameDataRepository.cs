﻿using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Data
{
    public class GameDataRepository
    {
        public Dictionary<GamePieceColor, Color> Colors { get; private set; }

        public GameDataRepository(ColorData colorData)
        {
            SetupColors(colorData);
        }

        private void SetupColors(ColorData colorData)
        {
            Colors = new Dictionary<GamePieceColor, Color>();

            foreach (ColorDataEntry colorDataEntry in colorData.GamePieceColors)
            {
                Colors.Add(colorDataEntry.Type, colorDataEntry.Color);
            }
        }
    }
}