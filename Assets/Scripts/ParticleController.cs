using System;
using Entities;
using Enums;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] private ParticleEffect _clearVFX;
    [SerializeField] private ParticleEffect _breakVFX;
    [SerializeField] private ParticleEffect _breakDoubleVFX;

    public void PlayParticleEffectAt(Vector2Int position, ParticleEffectType particleEffectType, int z = 0)
    {
        ParticleEffect effectPrefab = GetEffectPrefab(particleEffectType);
        Vector3 spawnPosition = new Vector3(position.x, position.y, z);
        ParticleEffect spawnedEffect = Instantiate(effectPrefab, spawnPosition, Quaternion.identity);

        spawnedEffect.Play();
    }

    private ParticleEffect GetEffectPrefab(ParticleEffectType particleEffectType)
    {
        return particleEffectType switch
        {
            ParticleEffectType.Clear => _clearVFX,
            ParticleEffectType.Break => _breakVFX,
            ParticleEffectType.DoubleBreak => _breakDoubleVFX,
            _ => throw new ArgumentOutOfRangeException(nameof(particleEffectType), particleEffectType, null)
        };
    }
}