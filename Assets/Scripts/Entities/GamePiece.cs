using System;
using System.Collections;
using Constants;
using Enums;
using Helpers;
using Services;
using StaticData;
using UnityEngine;

namespace Entities
{
    public class GamePiece : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private IStaticDataService _staticDataService;

        private bool _isMoving;
        private GamePieceType _type;
        private Vector2Int _position;
        private int _score;

        public GamePieceType Type => _type;
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

        public bool Bombed { get; set; } // TODO: fix later
        public int Score => _score;

        public event Action<GamePiece> OnPositionChanged;
        public event Action<GamePiece> OnStartMoving;

        public void Init(GamePieceStaticData gamePieceData, GamePieceColor color, int x, int y,
            Transform parentTransform, IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;

            _type = gamePieceData.Type;
            _score = gamePieceData.Score;

            SetColor(color);
            SetPosition(x, y);

            Transform trm = transform;
            trm.position = new Vector3(x, y, 0f);
            trm.rotation = Quaternion.identity;
            trm.SetParent(parentTransform);
        }

        public void Move(Vector2Int direction, int distance)
        {
            Vector2Int destination = Position + direction * distance;
            Move(destination);
        }

        public void Move(Vector2Int destination)
        {
            if (!_isMoving)
            {
                OnStartMoving?.Invoke(this);
                StartCoroutine(MoveRoutine(destination, Settings.TimeToMoveGamePiece));
            }
        }

        // TODO move out
        public void Destroy()
        {
            Destroy(gameObject);
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
                t = MovementHelper.ApplyInterpolation(t, _staticDataService.Settings.MoveInterpolationType);

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

            if (color != GamePieceColor.Undefined)
            {
                _spriteRenderer.color = _staticDataService.GetColorForGamePiece(color);
            }
        }
    }
}