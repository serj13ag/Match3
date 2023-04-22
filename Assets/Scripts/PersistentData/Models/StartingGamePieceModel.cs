using System;
using Enums;

namespace PersistentData.Models
{
    [Serializable]
    public class StartingGamePieceModel : StartingObjectModel
    {
        public GamePieceType GamePieceType;
        public GamePieceColor GamePieceColor;
    }
}