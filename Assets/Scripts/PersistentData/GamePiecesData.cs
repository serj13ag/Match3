using UnityEngine;

namespace PersistentData
{
    [CreateAssetMenu(fileName = "GamePiecesData", menuName = "Create Game Pieces")]
    public class GamePiecesData : ScriptableObject
    {
        public GamePieceModel[] GamePieces;
    }
}