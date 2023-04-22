using Interfaces;
using UnityEngine;

namespace Helpers
{
    public static class TileHelper
    {
        public static bool IsNeighbours(ITile tile1, ITile tile2)
        {
            if (Mathf.Abs(tile1.Position.x - tile2.Position.x) == 1
                && tile1.Position.y == tile2.Position.y)
            {
                return true;
            }

            if (Mathf.Abs(tile1.Position.y - tile2.Position.y) == 1
                && tile1.Position.x == tile2.Position.x)
            {
                return true;
            }

            return false;
        }
    }
}