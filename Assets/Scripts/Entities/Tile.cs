using UnityEngine;

namespace Entities
{
    public class Tile : MonoBehaviour
    {
        private Vector2Int _position;

        private Board _board;

        public void Init(int x, int y, Board board)
        {
            _position = new Vector2Int(x, y);
            _board = board;

            name = $"Tile {_position}";
            transform.SetParent(board.transform);
        }
    }
}