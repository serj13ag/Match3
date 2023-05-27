using System;
using Enums;

namespace StaticData.Models
{
    [Serializable]
    public class StartingGamePieceModel : StartingObjectModel
    {
        public GamePieceType GamePieceType;
        public GamePieceColor GamePieceColor;
    }
}