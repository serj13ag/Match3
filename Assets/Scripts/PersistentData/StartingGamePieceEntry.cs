using System;
using Entities;
using Enums;

namespace PersistentData
{
    [Serializable]
    public class StartingGamePieceEntry : StartingObjectEntry
    {
        public GamePiece GamePiecePrefab;
        public GamePieceColor GamePieceColor;
    }
}