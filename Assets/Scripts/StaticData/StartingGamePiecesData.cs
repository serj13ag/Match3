using StaticData.Models;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "StartingGamePiecesData", menuName = "Create Starting Game Pieces")]
    public class StartingGamePiecesData : ScriptableObject
    {
        public StartingGamePieceModel[] StartingGamePieces;
    }
}