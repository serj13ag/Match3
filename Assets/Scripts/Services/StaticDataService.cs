using System.Collections.Generic;
using System.Linq;
using Enums;
using StaticData;
using UnityEngine;

namespace Services
{
    public class StaticDataService
    {
        private const string TilesDataPath = "GameData/Tiles";
        private const string GamePiecesDataPath = "GameData/GamePieces";
        private const string ColorsDataPath = "GameData/ColorsData";
        private const string LevelDataPath = "GameData/LevelData";
        private const string MoveDataPath = "GameData/MoveData";

        public Dictionary<TileType, TileStaticData> Tiles { get; private set; }
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
            Tiles = LoadFilesFromResources<TileStaticData>(TilesDataPath)
                .ToDictionary(x => x.Type, x => x);
        }

        private void SetupGamePieces()
        {
            GamePieces = LoadFilesFromResources<GamePieceStaticData>(GamePiecesDataPath)
                .ToDictionary(x => x.Type, x => x);
        }

        private void SetupColors()
        {
            Colors = LoadFileFromResources<ColorsStaticData>(ColorsDataPath).GamePieceColors
                .ToDictionary(x => x.Type, x => x.Color);
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