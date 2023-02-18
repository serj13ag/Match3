using UnityEngine;

namespace Entities
{
    public class GamePiece : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private int _x;
        private int _y;

        public void Init(Color color)
        {
            SetColor(color);
        }

        public void MoveToPosition(int x, int y)
        {
            _x = x;
            _y = y;

            transform.position = new Vector3(x, y, 0);
            transform.rotation = Quaternion.identity;
        }

        private void SetColor(Color color)
        {
            _spriteRenderer.color = color;
        }
    }
}