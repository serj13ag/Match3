using System;
using Entities;
using Enums;

namespace Data
{
    [Serializable]
    public class StartingGamePieceEntry : StartingObjectEntry
    {
        public GamePiece GamePiecePrefab;
        public GamePieceColor GamePieceColor;
    }
}