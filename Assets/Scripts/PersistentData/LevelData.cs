﻿using Enums;
using UnityEngine;

namespace PersistentData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Create Level")]
    public class LevelData : ScriptableObject
    {
        public GamePieceColor[] AvailableColors;

        public StartingTilesData StartingTilesData;
        public StartingGamePiecesData StartingGamePiecesData;
    }
}