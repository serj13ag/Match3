using System;
using EventArguments;

namespace Services
{
    public interface ISettingsService : IService
    {
        bool SoundEnabled { get; }

        event EventHandler<SettingsChangedEventArgs> OnSettingsChanged;

        void SoundSetActive(bool activate);
    }
}