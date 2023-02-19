using System;
using System.Collections;
using Data;
using Enums;
using Helpers;
using UnityEngine;

namespace Entities
{
    public class GamePiece : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private GameDataRepository _gameDataRepository;
        private bool _isMoving;
        private Vector2Int _position;

        public GamePieceColor Color { get; private set; }

        public Vector2Int Position
        {
            get => _position;
            private set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPositionChanged?.Invoke(this);
                }
            }
        }

        public event Action<GamePiece> OnPositionChanged;

        public void Init(GamePieceColor color, int x, int y, GameDataRepository gameDataRepository,
            Transform parentTransform)
        {
            _gameDataRepository = gameDataRepository;

            SetColor(color);
            SetPosition(x, y);

            Transform trm = transform;
            trm.position = new Vector3(x, y, 0f);
            trm.rotation = Quaternion.identity;
            trm.SetParent(parentTransform);
        }

        public void Move(Vector2Int destination)
        {
            if (!_isMoving)
            {
                StartCoroutine(MoveRoutine(destination, Constants.TimeToMoveGamePiece));
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
                t = MovementHelper.ApplyInterpolation(t, _gameDataRepository.MoveInterpolationType);

                transform.position = Vector3.Lerp(startPosition, destinationPosition, t);

                yield return new WaitForEndOfFrame();
            }

            _isMoving = false;
            SetPosition(destination);
            transform.position = destinationPosition;
        }

        private void SetPosition(int x, int y)
        {
            SetPosition(new Vector2Int(x, y));
        }

        private void SetPosition(Vector2Int position)
        {
            Position = position;
        }

        private void SetColor(GamePieceColor color)
        {
            Color = color;

            _spriteRenderer.color = _gameDataRepository.Colors[color];
        }
    }
}