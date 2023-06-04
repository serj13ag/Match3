using System.Collections;
using Constants;
using UnityEngine;

namespace Services.Mono.Sound
{
    public class OneShotAudioSource : BaseAudioSource
    {
        public override void Init(AudioClip audioClip, float volume, float pitch = 1)
        {
            base.Init(audioClip, volume, pitch);

            StartCoroutine(DestroyAfterDelay(audioClip.length));
        }

        private IEnumerator DestroyAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay + Settings.Sound.AdditionalTimeBeforeDestroyAudioSource);
            Destroy(gameObject);
        }
    }
}