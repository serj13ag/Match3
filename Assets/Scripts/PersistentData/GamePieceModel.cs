using System;
using Entities;
using Enums;

namespace PersistentData
{
    [Serializable]
    public class GamePieceModel
    {
        public GamePieceType Type;
        public GamePiece GamePiecePrefab;
    }
}