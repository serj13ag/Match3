using System.Collections.Generic;
using Entities;

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
    }
}