using System;
using Controllers;
using Enums;
using Interfaces;
using PersistentData;
using UnityEngine;
using UnityEngine.Assertions;

namespace Entities.Tiles
{
    public abstract class BaseTile : MonoBehaviour, ITile
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private ParticleController _particleController;

        private TileType _tileType;
        private int _matchesTillBreak;
        private BreakableSpriteData[] _breakableSpriteData;

        private Vector2Int _position;

        public Vector2Int Position => _position;
        public abstract bool IsObstacle { get; }

        public event Action<ITile> OnClicked;
        public event Action<ITile> OnMouseEntered;
        public event Action OnMouseReleased;

        public void Init(int x, int y, Transform parentTransform, ParticleController particleController,
            TileModel tileModel)
        {
            _particleController = particleController;

            _tileType = tileModel.Type;
            _matchesTillBreak = tileModel.MatchesTillBreak;
            _breakableSpriteData = tileModel.BreakableSpriteData;

            _position = new Vector2Int(x, y);

            name = $"Tile {_position}";
            transform.SetParent(parentTransform);

            if (_tileType is TileType.Breakable or TileType.DoubleBreakable)
            {
                UpdateSprite();
            }
        }

        public void ProcessMatch()
        {
            Assert.IsTrue(_tileType != TileType.Obstacle);

            if (_tileType is TileType.Breakable or TileType.DoubleBreakable)
            {
                PlayVFX();

                _matchesTillBreak--;

                UpdateSprite();

                if (_matchesTillBreak == 0)
                {
                    _tileType = TileType.Normal;
                }
            }
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

        private void UpdateSprite()
        {
            _spriteRenderer.sprite = _breakableSpriteData[_matchesTillBreak].BreakableSprite;
            _spriteRenderer.color = _breakableSpriteData[_matchesTillBreak].BreakableColor;
        }

        private void PlayVFX()
        {
            ParticleEffectType effectType = _matchesTillBreak > 1
                ? ParticleEffectType.DoubleBreak
                : ParticleEffectType.Break;

            _particleController.PlayParticleEffectAt(_position, effectType);
        }
    }
}