using System;
using Entities;
using Enums;

namespace PersistentData.Models
{
    [Serializable]
    public class GamePieceModel
    {
        public GamePieceType Type;
        public GamePiece GamePiecePrefab;
        public int Score;
    }
}