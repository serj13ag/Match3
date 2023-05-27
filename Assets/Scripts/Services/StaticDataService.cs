using System.Collections.Generic;
using Enums;
using StaticData;
using StaticData.Models;
using UnityEngine;

namespace Services
{
    public class StaticDataService
    {
        private const string TilesDataPath = "GameData/TilesData";
        private const string GamePiecesDataPath = "GameData/GamePiecesData";
        private const string ColorDataPath = "GameData/ColorData";
        private const string LevelDataPath = "GameData/LevelData";
        private const string MoveDataPath = "GameData/MoveData";

        public Dictionary<TileType, TileModel> Tiles { get; private set; }
        public Dictionary<GamePieceType, GamePieceModel> GamePieces { get; private set; }
        public Dictionary<GamePieceColor, Color> Colors { get; private set; }

        public LevelData LevelData { get; }
        public MoveInterpolationType MoveInterpolationType { get; }

        public StaticDataService()
        {
            SetupTiles();
            SetupGamePieces();
            SetupColors();

            LevelData = LoadFromResources<LevelData>(LevelDataPath);
            MoveInterpolationType = LoadFromResources<MoveData>(MoveDataPath).MoveInterpolationType;
        }

        private void SetupTiles()
        {
            TilesData tilesData = LoadFromResources<TilesData>(TilesDataPath);
            Tiles = new Dictionary<TileType, TileModel>();

            foreach (TileModel tileModel in tilesData.Tiles)
            {
                Tiles.Add(tileModel.Type, tileModel);
            }
        }

        private void SetupGamePieces()
        {
            GamePiecesData gamePiecesData = LoadFromResources<GamePiecesData>(GamePiecesDataPath);
            GamePieces = new Dictionary<GamePieceType, GamePieceModel>();

            foreach (GamePieceModel gamePieceModel in gamePiecesData.GamePieces)
            {
                GamePieces.Add(gamePieceModel.Type, gamePieceModel);
            }
        }

        private void SetupColors()
        {
            ColorData colorData = LoadFromResources<ColorData>(ColorDataPath);
            Colors = new Dictionary<GamePieceColor, Color>();

            foreach (ColorDataModel colorDataEntry in colorData.GamePieceColors)
            {
                Colors.Add(colorDataEntry.Type, colorDataEntry.Color);
            }
        }

        private static T LoadFromResources<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }
    }
}