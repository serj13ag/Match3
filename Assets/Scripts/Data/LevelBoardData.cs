using System;
using System.Collections.Generic;
using Entities;
using Enums;
using Interfaces;
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

        public LevelBoardData(string levelName, ITile[,] tiles, GamePiece[,] gamePieces)
        {
            LevelName = levelName;

            Tiles = new List<TileSaveData>();
            foreach (ITile tile in tiles)
            {
                Tiles.Add(new TileSaveData(tile.Type, tile.Position));
            }

            GamePieces = new List<GamePieceSaveData>();
            foreach (GamePiece gamePiece in gamePieces)
            {
                if (gamePiece != null)
                {
                    GamePieces.Add(new GamePieceSaveData(gamePiece.Type, gamePiece.Position, gamePiece.Color));
                }
            }
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