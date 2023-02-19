using System;
using UnityEngine;

namespace Entities
{
    public class Tile : MonoBehaviour
    {
        public event Action<Tile> OnClicked;
        public event Action<Tile> OnMouseEntered;
        public event Action OnMouseReleased;

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
            OnClicked?.Invoke(this);
        }

        private void OnMouseEnter()
        {
            OnMouseEntered?.Invoke(this);
        }

        private void OnMouseUp()
        {
            OnMouseReleased?.Invoke();
        }
    }
}