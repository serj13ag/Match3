using Enums;

namespace Services.Mono.Sound
{
    public interface ISoundMonoService : IService
    {
        void Init(IRandomService randomService, ISettingsService settingsService);

        void PlayBackgroundMusic();
        void PlaySound(SoundType soundType);
        void AdStarted();
        void AdEnded();
    }
}