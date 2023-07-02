using Enums;

namespace Services.Mono.Sound
{
    public interface ISoundMonoService
    {
        void Init(IRandomService randomService);

        void SwitchSoundMode();

        void PlayBackgroundMusic();
        void PlaySound(SoundType soundType);
    }
}