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

        private void OnMouseDown()
        {
            _board.ClickTile(this);
        }

        private void OnMouseEnter()
        {
            _board.DragToTile(this);
        }

        private void OnMouseUp()
        {
            _board.ReleaseTile();
        }
    }
}