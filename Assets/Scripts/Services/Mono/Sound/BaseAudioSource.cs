using UnityEngine;

namespace Services.Mono.Sound
{
    public abstract class BaseAudioSource : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        protected AudioSource AudioSource => _audioSource;

        public virtual void Init(AudioClip audioClip, float volume, float pitch = 1f)
        {
            _audioSource.clip = audioClip;
            _audioSource.volume = volume;
            _audioSource.pitch = pitch;

            _audioSource.Play();
        }
    }
}