using System.Collections.Generic;
using Enums;
using StaticData;
using UnityEngine;

namespace Services
{
    public interface IStaticDataService : IService
    {
        SettingsStaticData Settings { get; }
        IEnumerable<string> PuzzleLevelNames { get; }

        TileStaticData GetDataForTile(TileType tileType);
        GamePieceStaticData GetDataForGamePiece(GamePieceType gamePieceType);
        Color GetColorForGamePiece(GamePieceColor gamePieceColor);
        LevelStaticData GetDataForLevel(string levelName);
        ParticleEffectStaticData GetDataForParticleEffect(ParticleEffectType particleEffectType);
        TextAsset GetDataForLanguage(LanguageType languageType);
    }
}