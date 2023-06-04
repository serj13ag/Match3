using System.Collections;
using Constants;
using UnityEngine;

namespace Services.Mono.Sound
{
    public class OneShotAudioSource : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        public void Init(AudioClip audioClip, float volume, float pitch)
        {
            _audioSource.clip = audioClip;
            _audioSource.volume = volume;
            _audioSource.pitch = pitch;

            _audioSource.Play();

            StartCoroutine(DestroyAfterDelay(audioClip.length));
        }

        private IEnumerator DestroyAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay + Settings.Sound.AdditionalTimeBeforeDestroyAudioSource);
            Destroy(gameObject);
        }
    }
}