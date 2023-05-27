using UnityEngine;

namespace StaticData.StartingData
{
    [CreateAssetMenu(fileName = "StartingGamePiecesData", menuName = "Create Starting Game Pieces")]
    public class StartingGamePiecesStaticData : ScriptableObject
    {
        public StartingGamePieceStaticData[] StartingGamePieces;
    }
}