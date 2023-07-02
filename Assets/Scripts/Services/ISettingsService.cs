namespace Services
{
    public interface ISettingsService
    {
        bool SoundEnabled { get; }

        void SoundSetActive(bool activate);
    }
}