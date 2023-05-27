using UnityEngine;

namespace StaticData.StartingData
{
    [CreateAssetMenu(fileName = "StartingGamePiecesData", menuName = "StaticData/StartingGamePieces")]
    public class StartingGamePiecesStaticData : ScriptableObject
    {
        public StartingGamePieceStaticData[] StartingGamePieces;
    }
}