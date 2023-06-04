namespace Services.Mono.Sound
{
    public class LoopAudioSource : BaseAudioSource
    {
        private void Update()
        {
            if (!AudioSource.isPlaying)
            {
                AudioSource.Stop();
                AudioSource.Play();
            }
        }
    }
}