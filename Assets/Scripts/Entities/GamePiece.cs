using UnityEngine;

namespace Entities
{
    public class GamePiece : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Vector2Int _position;

        public void Init(Color color)
        {
            SetColor(color);
        }

        public void MoveToPosition(int x, int y)
        {
            _position = new Vector2Int(x, y);

            transform.position = new Vector3(x, y, 0);
            transform.rotation = Quaternion.identity;
        }

        private void SetColor(Color color)
        {
            _spriteRenderer.color = color;
        }
    }
}