using Entities;
using Enums;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "GamePieceStaticData", menuName = "StaticData/GamePiece")]
    public class GamePieceStaticData : ScriptableObject
    {
        public GamePieceType Type;
        public GamePiece Prefab;
        public int Score;
    }
}