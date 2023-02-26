using UnityEngine;

namespace Entities
{
    public class ParticleEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] _particleSystems;
        [SerializeField] private float _lifeTime = 1f;

        public void Play()
        {
            foreach (ParticleSystem particle in _particleSystems)
            {
                particle.Stop();
                particle.Play();
            }

            Destroy(gameObject, _lifeTime);
        }
    }
}