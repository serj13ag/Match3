using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class LevelBoardData
    {
        public string LevelName { get; set; }
        public List<TileSaveData> Tiles { get; set; }
        public List<GamePieceSaveData> GamePieces { get; set; }
        public int Score { get; set; }
    }

    [Serializable]
    public class GamePieceSaveData
    {
        public GamePieceType Type { get; set; }
        public Vector2Int Position { get; set; }
        public GamePieceColor Color { get; set; }

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
        public TileType Type { get; set; }
        public Vector2Int Position { get; set; }

        public TileSaveData(TileType type, Vector2Int position)
        {
            Type = type;
            Position = position;
        }
    }
}