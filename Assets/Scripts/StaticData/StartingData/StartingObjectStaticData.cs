using System;
using Enums;

namespace StaticData.StartingData
{
    [Serializable]
    public class StartingTileStaticData : StartingObjectStaticData
    {
        public TileType Type;
    }

    [Serializable]
    public class StartingGamePieceStaticData : StartingObjectStaticData
    {
        public GamePieceType Type;
        public GamePieceColor Color;
    }

    [Serializable]
    public abstract class StartingObjectStaticData
    {
        public int X;
        public int Y;
    }
}