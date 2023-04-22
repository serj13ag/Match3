﻿using PersistentData.Models;
using UnityEngine;

namespace PersistentData
{
    [CreateAssetMenu(fileName = "StartingGamePiecesData", menuName = "Create Starting Game Pieces")]
    public class StartingGamePiecesData : ScriptableObject
    {
        public StartingGamePieceModel[] StartingGamePieces;
    }
}