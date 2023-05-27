using System;
using Entities;
using Enums;

namespace StaticData.Models
{
    [Serializable]
    public class GamePieceModel
    {
        public GamePieceType Type;
        public GamePiece GamePiecePrefab;
        public int Score;
    }
}