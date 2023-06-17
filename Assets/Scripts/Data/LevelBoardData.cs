using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class LevelBoardData
    {
        public string LevelName;
        public List<TileSaveData> Tiles;
        public List<GamePieceSaveData> GamePieces;

        public LevelBoardData(string levelName)
        {
            LevelName = levelName;
        }
    }

    [Serializable]
    public class GamePieceSaveData
    {
        public GamePieceType Type;
        public Vector2Int Position;
        public GamePieceColor Color;

        public GamePieceSaveData(GamePieceType type, Vector2Int position, GamePieceColor color)
        {
            Type = type;
            Position = position;
            Color = color;
        }
    }

    [Serializable]
    public class TileSaveData
    {
        public TileType Type;
        public Vector2Int Position;

        public TileSaveData(TileType type, Vector2Int position)
        {
            Type = type;
            Position = position;
        }
    }
}