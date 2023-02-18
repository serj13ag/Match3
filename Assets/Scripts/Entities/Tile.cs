using UnityEngine;

namespace Entities
{
    public class Tile : MonoBehaviour
    {
        private int _x;
        private int _y;

        private Board _board;

        public void Init(int x, int y, Board board)
        {
            _x = x;
            _y = y;
            _board = board;

            name = $"Tile ({x}, {y})";
            transform.SetParent(board.transform);
        }
    }
}