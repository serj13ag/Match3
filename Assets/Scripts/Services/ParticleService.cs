using Entities;
using Enums;
using UnityEngine;

namespace Services
{
    public class ParticleService : IParticleService
    {
        private readonly IStaticDataService _staticDataService;

        public ParticleService(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void PlayParticleEffectAt(Vector2Int position, ParticleEffectType particleEffectType, int z = 0, Color? color = null)
        {
            ParticleEffect effectPrefab = GetEffectPrefab(particleEffectType);
            Vector3 spawnPosition = new Vector3(position.x, position.y, z);
            ParticleEffect spawnedEffect = Object.Instantiate(effectPrefab, spawnPosition, Quaternion.identity);

            spawnedEffect.Play(color);
        }

        private ParticleEffect GetEffectPrefab(ParticleEffectType particleEffectType)
        {
            return _staticDataService.GetDataForParticleEffect(particleEffectType).Prefab;
        }
    }
}