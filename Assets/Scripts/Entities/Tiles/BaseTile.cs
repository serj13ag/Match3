using System;
using Enums;
using Interfaces;
using Services;
using Services.Mono;
using StaticData;
using UnityEngine;

namespace Entities.Tiles
{
    public abstract class BaseTile : MonoBehaviour, ITile
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private TileType _type;
        private Vector2Int _position;

        public TileType Type => _type;
        public Vector2Int Position => _position;
        public abstract bool IsObstacle { get; }

        protected SpriteRenderer SpriteRenderer => _spriteRenderer;

        public event Action<ITile> OnClicked;
        public event Action<ITile> OnMouseEntered;
        public event Action OnMouseReleased;

        public virtual void Init(TileStaticData tileData, int x, int y, Transform parentTransform,
            ParticleService particleService)
        {
            _type = tileData.Type;
            _position = new Vector2Int(x, y);

            name = $"Tile {_position}";
            transform.SetParent(parentTransform);
        }

        public abstract void ProcessMatch();

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