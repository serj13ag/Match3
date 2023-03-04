using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "StartingGamePiecesData", menuName = "Create Starting Game Pieces")]
    public class StartingGamePiecesData : ScriptableObject
    {
        public StartingGamePieceEntry[] StartingGamePieces;
    }
}