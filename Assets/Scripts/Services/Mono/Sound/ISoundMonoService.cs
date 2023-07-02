using Enums;

namespace Services.Mono.Sound
{
    public interface ISoundMonoService
    {
        void Init(IRandomService randomService, ISettingsService settingsService);

        void SwitchSoundMode();

        void PlayBackgroundMusic();
        void PlaySound(SoundType soundType);
    }
}