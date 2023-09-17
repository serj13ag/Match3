using System.Collections.Generic;
using System.Linq;
using Constants;
using Enums;
using StaticData;
using StaticData.Shop;
using UnityEngine;

namespace Services
{
    public class StaticDataService : IStaticDataService
    {
        private readonly Dictionary<TileType, TileStaticData> _tiles;
        private readonly Dictionary<GamePieceType, GamePieceStaticData> _gamePieces;
        private readonly Dictionary<GamePieceColor, Color> _colors;
        private readonly Dictionary<string, LevelStaticData> _levels;
        private readonly Dictionary<ParticleEffectType, ParticleEffectStaticData> _particleEffects;
        private readonly Dictionary<LanguageType, LanguageStaticData> _languages;

        private readonly Dictionary<string, BackgroundShopItemStaticData> _backgroundShopItems;

        private readonly SettingsStaticData _settings;

        public SettingsStaticData Settings => _settings;

        public IEnumerable<string> PuzzleLevelNames => _settings.PuzzleLevels.Select(x => x.LevelName);

        public List<LanguageType> AvailableLanguages => _languages.Keys.ToList();

        public IEnumerable<BackgroundShopItemStaticData> BackgroundShopItems => _backgroundShopItems.Values;

        public StaticDataService()
        {
            _tiles = LoadFilesFromResources<TileStaticData>(AssetPaths.TilesDataPath)
                .ToDictionary(x => x.Type, x => x);

            _gamePieces = LoadFilesFromResources<GamePieceStaticData>(AssetPaths.GamePiecesDataPath)
                .ToDictionary(x => x.Type, x => x);

            _colors = LoadFileFromResources<ColorsStaticData>(AssetPaths.ColorsDataPath)
                .GamePieceColors
                .ToDictionary(x => x.Type, x => x.Color);

            _levels = LoadFilesFromResources<LevelStaticData>(AssetPaths.LevelsDataPath)
                .ToDictionary(x => x.LevelName, x => x);

            _settings = LoadFileFromResources<SettingsStaticData>(AssetPaths.SettingsDataPath);

            _particleEffects = LoadFilesFromResources<ParticleEffectStaticData>(AssetPaths.ParticleEffectsDataPath)
                .ToDictionary(x => x.Type, x => x);

            _languages = LoadFileFromResources<LanguagesStaticData>(AssetPaths.LanguagesDataPath)
                .Languages
                .ToDictionary(x => x.Type, x => x);

            _backgroundShopItems =
                LoadFilesFromResources<BackgroundShopItemStaticData>(AssetPaths.BackgroundShopItemsDataPath)
                    .ToDictionary(x => x.Code, x => x);
        }

        public TileStaticData GetDataForTile(TileType tileType)
        {
            if (_tiles.TryGetValue(tileType, out TileStaticData tileStaticData))
            {
                return tileStaticData;
            }

            return null;
        }

        public GamePieceStaticData GetDataForGamePiece(GamePieceType gamePieceType)
        {
            if (_gamePieces.TryGetValue(gamePieceType, out GamePieceStaticData gamePieceStaticData))
            {
                return gamePieceStaticData;
            }

            return null;
        }

        public Color GetColorForGamePiece(GamePieceColor gamePieceColor)
        {
            if (_colors.TryGetValue(gamePieceColor, out Color color))
            {
                return color;
            }

            return Color.white;
        }

        public LevelStaticData GetDataForLevel(string levelName)
        {
            if (_levels.TryGetValue(levelName, out LevelStaticData levelStaticData))
            {
                return levelStaticData;
            }

            return null;
        }

        public ParticleEffectStaticData GetDataForParticleEffect(ParticleEffectType particleEffectType)
        {
            if (_particleEffects.TryGetValue(particleEffectType, out ParticleEffectStaticData particleEffectStaticData))
            {
                return particleEffectStaticData;
            }

            return null;
        }

        public LanguageStaticData GetDataForLanguage(LanguageType languageType)
        {
            if (_languages.TryGetValue(languageType, out LanguageStaticData languageStaticData))
            {
                return languageStaticData;
            }

            return null;
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