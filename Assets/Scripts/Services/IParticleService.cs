using Enums;
using UnityEngine;

namespace Services
{
    public interface IParticleService
    {
        void PlayParticleEffectAt(Vector2Int position, ParticleEffectType particleEffectType, int z = 0);
    }
}