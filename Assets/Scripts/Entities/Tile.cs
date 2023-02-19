using System;
using UnityEngine;

namespace Entities
{
    public class Tile : MonoBehaviour
    {
        public Vector2Int Position { get; private set; }

        public event Action<Tile> OnClicked;
        public event Action<Tile> OnMouseEntered;
        public event Action OnMouseReleased;

        public void Init(int x, int y, Transform parentTransform)
        {
            Position = new Vector2Int(x, y);

            name = $"Tile {Position}";
            transform.SetParent(parentTransform);
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