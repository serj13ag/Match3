using System;
using Enums;

namespace StaticData.StartingData
{
    [Serializable]
    public class StartingTileStaticData : StartingObjectStaticData
    {
        public TileType TileType;
    }

    [Serializable]
    public class StartingGamePieceStaticData : StartingObjectStaticData
    {
        public GamePieceType GamePieceType;
        public GamePieceColor GamePieceColor;
    }

    [Serializable]
    public abstract class StartingObjectStaticData
    {
        public int X;
        public int Y;
    }
}