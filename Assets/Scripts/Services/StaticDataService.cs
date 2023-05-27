using System.Collections.Generic;
using System.Linq;
using Enums;
using StaticData;
using StaticData.Models;
using UnityEngine;

namespace Services
{
    public class StaticDataService
    {
        private const string TilesDataPath = "GameData/TilesData";
        private const string GamePiecesDataPath = "GameData/GamePieces";
        private const string ColorDataPath = "GameData/ColorData";
        private const string LevelDataPath = "GameData/LevelData";
        private const string MoveDataPath = "GameData/MoveData";

        public Dictionary<TileType, TileModel> Tiles { get; private set; }
        public Dictionary<GamePieceType, GamePieceStaticData> GamePieces { get; private set; }
        public Dictionary<GamePieceColor, Color> Colors { get; private set; }

        public LevelData LevelData { get; }
        public MoveInterpolationType MoveInterpolationType { get; }

        public StaticDataService()
        {
            SetupTiles();
            SetupGamePieces();
            SetupColors();

            LevelData = LoadFileFromResources<LevelData>(LevelDataPath);
            MoveInterpolationType = LoadFileFromResources<MoveData>(MoveDataPath).MoveInterpolationType;
        }

        private void SetupTiles()
        {
            TilesData tilesData = LoadFileFromResources<TilesData>(TilesDataPath);
            Tiles = new Dictionary<TileType, TileModel>();

            foreach (TileModel tileModel in tilesData.Tiles)
            {
                Tiles.Add(tileModel.Type, tileModel);
            }
        }

        private void SetupGamePieces()
        {
            GamePieces = LoadFilesFromResources<GamePieceStaticData>(GamePiecesDataPath)
                .ToDictionary(x => x.Type, x => x);
        }

        private void SetupColors()
        {
            ColorData colorData = LoadFileFromResources<ColorData>(ColorDataPath);
            Colors = new Dictionary<GamePieceColor, Color>();

            foreach (ColorDataModel colorDataEntry in colorData.GamePieceColors)
            {
                Colors.Add(colorDataEntry.Type, colorDataEntry.Color);
            }
        }

        private static T LoadFileFromResources<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }

        private static T[] LoadFilesFromResources<T>(string path) where T : Object
        {
            return Resources.LoadAll<T>(path);
        }
    }
}