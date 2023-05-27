using Enums;
using Services.Mono;
using StaticData.Models;
using UnityEngine;
using UnityEngine.Assertions;

namespace Entities.Tiles
{
    public class BreakableTile : BaseTile
    {
        private ParticleMonoService _particleMonoService;

        private int _matchesTillBreak;
        private BreakableSpriteModel[] _breakableSpriteData;

        public override bool IsObstacle => false;

        public override void Init(int x, int y, Transform parentTransform, ParticleMonoService particleMonoService,
            TileModel tileModel)
        {
            base.Init(x, y, parentTransform, particleMonoService, tileModel);

            _particleMonoService = particleMonoService;

            Assert.IsTrue(tileModel.MatchesTillBreak > 0);

            _matchesTillBreak = tileModel.MatchesTillBreak;
            _breakableSpriteData = tileModel.BreakableSpriteData;

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
            SpriteRenderer.sprite = _breakableSpriteData[_matchesTillBreak].BreakableSprite;
            SpriteRenderer.color = _breakableSpriteData[_matchesTillBreak].BreakableColor;
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