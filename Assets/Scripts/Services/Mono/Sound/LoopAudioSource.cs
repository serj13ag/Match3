using UnityEngine;

namespace Services.Mono.Sound
{
    public class LoopAudioSource : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        public void Init(AudioClip audioClip, float volume, float pitch = 1f)
        {
            _audioSource.clip = audioClip;
            _audioSource.volume = volume;
            _audioSource.pitch = pitch;

            _audioSource.Play();
        }

        private void Update()
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.Stop();
                _audioSource.Play();
            }
        }
    }
}