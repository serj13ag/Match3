using System.Collections.Generic;
using System.Linq;
using Enums;
using StaticData;
using UnityEngine;

namespace Services
{
    public class StaticDataService
    {
        private const string TilesDataPath = "GameData/Tiles/";
        private const string GamePiecesDataPath = "GameData/GamePieces/";
        private const string LevelsDataPath = "GameData/Levels/";

        private const string ColorsDataPath = "GameData/ColorsData";
        private const string SettingsDataPath = "GameData/SettingsData";

        public Dictionary<TileType, TileStaticData> Tiles { get; }
        public Dictionary<GamePieceType, GamePieceStaticData> GamePieces { get; }
        public Dictionary<GamePieceColor, Color> Colors { get; }

        public Dictionary<string, LevelStaticData> Levels { get; }

        public SettingsStaticData Settings { get; }

        public StaticDataService()
        {
            Tiles = LoadFilesFromResources<TileStaticData>(TilesDataPath)
                .ToDictionary(x => x.Type, x => x);

            GamePieces = LoadFilesFromResources<GamePieceStaticData>(GamePiecesDataPath)
                .ToDictionary(x => x.Type, x => x);

            Colors = LoadFileFromResources<ColorsStaticData>(ColorsDataPath)
                .GamePieceColors
                .ToDictionary(x => x.Type, x => x.Color);

            Levels = LoadFilesFromResources<LevelStaticData>(LevelsDataPath)
                .ToDictionary(x => x.LevelName, x => x);

            Settings = LoadFileFromResources<SettingsStaticData>(SettingsDataPath);
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