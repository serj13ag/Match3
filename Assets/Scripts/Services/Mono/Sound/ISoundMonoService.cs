using Enums;

namespace Services.Mono.Sound
{
    public interface ISoundMonoService
    {
        void Init(IRandomService randomService);
        void PlayBackgroundMusic();
        void PlaySound(SoundType soundType);
    }
}