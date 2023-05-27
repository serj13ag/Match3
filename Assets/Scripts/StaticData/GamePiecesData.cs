using StaticData.Models;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "GamePiecesData", menuName = "Create Game Pieces")]
    public class GamePiecesData : ScriptableObject
    {
        public GamePieceModel[] GamePieces;
    }
}