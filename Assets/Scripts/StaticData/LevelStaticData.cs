using Enums;
using StaticData.StartingData;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public string LevelName;
        public GamePieceColor[] AvailableColors;

        public StartingTilesStaticData StartingTiles;
        public StartingGamePiecesStaticData StartingGamePieces;

        public int ScoreGoal;
        public int MovesLeft;
    }
}