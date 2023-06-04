using System.Collections.Generic;
using System.Linq;
using Constants;
using Enums;
using StaticData;
using UnityEngine;

namespace Services
{
    public class StaticDataService
    {
        public Dictionary<TileType, TileStaticData> Tiles { get; }
        public Dictionary<GamePieceType, GamePieceStaticData> GamePieces { get; }
        public Dictionary<GamePieceColor, Color> Colors { get; }

        public Dictionary<string, LevelStaticData> Levels { get; }

        public SettingsStaticData Settings { get; }

        public Dictionary<ParticleEffectType, ParticleEffectStaticData> ParticleEffects { get; }

        public StaticDataService()
        {
            Tiles = LoadFilesFromResources<TileStaticData>(AssetPaths.TilesDataPath)
                .ToDictionary(x => x.Type, x => x);

            GamePieces = LoadFilesFromResources<GamePieceStaticData>(AssetPaths.GamePiecesDataPath)
                .ToDictionary(x => x.Type, x => x);

            Colors = LoadFileFromResources<ColorsStaticData>(AssetPaths.ColorsDataPath)
                .GamePieceColors
                .ToDictionary(x => x.Type, x => x.Color);

            Levels = LoadFilesFromResources<LevelStaticData>(AssetPaths.LevelsDataPath)
                .ToDictionary(x => x.LevelName, x => x);

            Settings = LoadFileFromResources<SettingsStaticData>(AssetPaths.SettingsDataPath);

            ParticleEffects = LoadFilesFromResources<ParticleEffectStaticData>(AssetPaths.ParticleEffectsDataPath)
                .ToDictionary(x => x.Type, x => x);
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