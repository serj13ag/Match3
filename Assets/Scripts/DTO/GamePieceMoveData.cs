using Entities;
using UnityEngine;

namespace DTO
{
    public struct GamePieceMoveData
    {
        public GamePiece GamePiece { get; }
        public Vector2Int Direction { get; }
        public int Distance { get; }

        public GamePieceMoveData(GamePiece gamePiece, Vector2Int direction, int distance)
        {
            GamePiece = gamePiece;
            Direction = direction;
            Distance = distance;
        }
    }
}