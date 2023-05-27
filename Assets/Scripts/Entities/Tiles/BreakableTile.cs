using Enums;
using Services.Mono;
using StaticData;
using UnityEngine;
using UnityEngine.Assertions;

namespace Entities.Tiles
{
    public class BreakableTile : BaseTile
    {
        private ParticleMonoService _particleMonoService;

        private int _matchesTillBreak;
        private BreakableSpriteStaticData[] _breakableSpriteData;

        public override bool IsObstacle => false;

        public override void Init(TileStaticData tileData, int x, int y, Transform parentTransform,
            ParticleMonoService particleMonoService)
        {
            base.Init(tileData, x, y, parentTransform, particleMonoService);

            _particleMonoService = particleMonoService;

            Assert.IsTrue(tileData.MatchesTillBreak > 0);

            _matchesTillBreak = tileData.MatchesTillBreak;
            _breakableSpriteData = tileData.BreakableSprites;

            UpdateSprite();
        }

        public override void ProcessMatch()
        {
            Assert.IsFalse(IsObstacle);

            if (_matchesTillBreak > 0)
            {
                PlayVFX();

                _matchesTillBreak--;

                UpdateSprite();
            }
        }

        private void UpdateSprite()
        {
            SpriteRenderer.sprite = _breakableSpriteData[_matchesTillBreak].Sprite;
            SpriteRenderer.color = _breakableSpriteData[_matchesTillBreak].Color;
        }

        private void PlayVFX()
        {
            ParticleEffectType effectType = _matchesTillBreak > 1
                ? ParticleEffectType.DoubleBreak
                : ParticleEffectType.Break;

            _particleMonoService.PlayParticleEffectAt(Position, effectType);
        }
    }
}