using System.Collections;
using UnityEngine;

namespace Entities
{
    public class GamePiece : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Vector2Int _position;
        private bool _isMoving;

        public void Init(Color color, int x, int y)
        {
            SetColor(color);
            SetPosition(x, y);

            transform.position = new Vector3(x, y, 0f);
            transform.rotation = Quaternion.identity;
        }

        private void Update()
        {
            // Debug
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Move(new Vector2Int(_position.x + 1, _position.y), Constants.TimeToMoveGamePiece);
            }

            // Debug
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Move(new Vector2Int(_position.x - 1, _position.y), Constants.TimeToMoveGamePiece);
            }
        }

        private void Move(Vector2Int destination, float timeToMove)
        {
            if (!_isMoving)
            {
                StartCoroutine(MoveRoutine(destination, timeToMove));
            }
        }

        private IEnumerator MoveRoutine(Vector2Int destination, float timeToMove)
        {
            _isMoving = true;

            Vector3 startPosition = transform.position;
            Vector3 destinationPosition = new Vector3(destination.x, destination.y, 0f);

            float timeLeft = timeToMove;

            while (timeLeft > 0f)
            {
                timeLeft -= Time.deltaTime;
                float t = (timeToMove - timeLeft) / timeToMove;

                transform.position = Vector3.Lerp(startPosition, destinationPosition, t);

                yield return new WaitForEndOfFrame();
            }

            SetPosition(destination);
            transform.position = destinationPosition;
            _isMoving = false;
        }

        private void SetPosition(int x, int y)
        {
            SetPosition(new Vector2Int(x, y));
        }

        private void SetPosition(Vector2Int position)
        {
            _position = position;
        }

        private void SetColor(Color color)
        {
            _spriteRenderer.color = color;
        }
    }
}