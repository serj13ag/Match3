using System;
using Controllers;
using Data;
using Enums;
using UnityEngine;
using UnityEngine.Assertions;

namespace Entities
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private TileType _tileType = TileType.Normal;

        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField] private int _matchesTillBreak;
        [SerializeField] private BreakableSpriteData[] _breakableSpriteData;

        private ParticleController _particleController;

        public Vector2Int Position { get; private set; }
        public TileType TileType => _tileType;

        public event Action<Tile> OnClicked;
        public event Action<Tile> OnMouseEntered;
        public event Action OnMouseReleased;

        public void Init(int x, int y, Transform parentTransform, ParticleController particleController)
        {
            _particleController = particleController;

            Position = new Vector2Int(x, y);

            name = $"Tile {Position}";
            transform.SetParent(parentTransform);

            if (_tileType == TileType.Breakable)
            {
                UpdateSprite();
            }
        }

        public void ProcessMatch()
        {
            Assert.IsTrue(_tileType != TileType.Obstacle);

            if (_tileType != TileType.Breakable)
            {
                return;
            }

            PlayVFX();

            _matchesTillBreak--;

            UpdateSprite();

            if (_matchesTillBreak == 0)
            {
                _tileType = TileType.Normal;
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

            _particleController.PlayParticleEffectAt(Position, effectType);
        }
    }
}