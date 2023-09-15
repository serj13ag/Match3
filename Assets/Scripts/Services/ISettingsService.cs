using System;
using Enums;
using EventArguments;

namespace Services
{
    public interface ISettingsService : IService
    {
        bool MusicEnabled { get; }
        bool SoundEnabled { get; }
        LanguageType Language { get; }

        event EventHandler<SettingsChangedEventArgs> OnSettingsChanged;

        void InitGameSettings();
        void MusicSetActive(bool activate);
        void SoundSetActive(bool activate);
        void SetLanguage(LanguageType language);
    }
}