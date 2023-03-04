using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace Helpers
{
    public static class BoardHelper
    {
        public static HashSet<int> GetColumnIndexes(HashSet<GamePiece> gamePieces)
        {
            var columns = new HashSet<int>();

            foreach (GamePiece gamePiece in gamePieces)
            {
                columns.Add(gamePiece.Position.x);
            }

            return columns;
        }

        public static bool IsOutOfBounds(Vector2Int position, Vector2Int boardSize)
        {
            return position.x < 0 || position.x > boardSize.x - 1 ||
                   position.y < 0 || position.y > boardSize.y - 1;
        }
    }
}