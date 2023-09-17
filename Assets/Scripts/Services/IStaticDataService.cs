using System.Collections.Generic;
using Enums;
using StaticData;
using StaticData.Shop;
using UnityEngine;

namespace Services
{
    public interface IStaticDataService : IService
    {
        SettingsStaticData Settings { get; }
        IEnumerable<string> PuzzleLevelNames { get; }
        List<LanguageType> AvailableLanguages { get; }
        IEnumerable<BackgroundShopItemStaticData> BackgroundShopItems { get; }

        TileStaticData GetDataForTile(TileType tileType);
        GamePieceStaticData GetDataForGamePiece(GamePieceType gamePieceType);
        Color GetColorForGamePiece(GamePieceColor gamePieceColor);
        LevelStaticData GetDataForLevel(string levelName);
        ParticleEffectStaticData GetDataForParticleEffect(ParticleEffectType particleEffectType);
        LanguageStaticData GetDataForLanguage(LanguageType languageType);
    }
}