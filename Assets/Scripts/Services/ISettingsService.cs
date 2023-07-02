using System;

namespace Services
{
    public interface ISettingsService
    {
        bool SoundEnabled { get; }

        event EventHandler<EventArgs> OnSettingsChanged;

        void SoundSetActive(bool activate);
    }
}