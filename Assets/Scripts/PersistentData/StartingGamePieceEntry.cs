using System;
using Enums;

namespace PersistentData
{
    [Serializable]
    public class StartingGamePieceEntry : StartingObjectEntry
    {
        public GamePieceType GamePieceType;
        public GamePieceColor GamePieceColor;
    }
}