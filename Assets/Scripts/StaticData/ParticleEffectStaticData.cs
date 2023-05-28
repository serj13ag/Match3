using Entities;
using Enums;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "ParticleEffectData", menuName = "StaticData/ParticleEffect")]
    public class ParticleEffectStaticData : ScriptableObject
    {
        public ParticleEffectType Type;
        public ParticleEffect Prefab;
    }
}