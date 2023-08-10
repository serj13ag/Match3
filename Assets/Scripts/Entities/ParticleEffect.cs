using UnityEngine;

namespace Entities
{
    public class ParticleEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] _particleSystems;
        [SerializeField] private float _lifeTime = 1f;

        public void Play(Color? color)
        {
            foreach (ParticleSystem particle in _particleSystems)
            {
                if (color != null)
                {
                    ParticleSystem.MainModule particleMain = particle.main;
                    particleMain.startColor = new ParticleSystem.MinMaxGradient(color.Value);
                }

                particle.Stop();
                particle.Play();
            }

            Destroy(gameObject, _lifeTime);
        }
    }
}