using System;
using Data;
using Enums;
using EventArguments;
using Interfaces;

namespace Services
{
    public class SettingsService : ISettingsService, IPersistentDataReader
    {
        private readonly IPersistentDataService _persistentDataService;

        public bool MusicEnabled => _persistentDataService.GameSettings.MusicEnabled;
        public bool SoundEnabled => _persistentDataService.GameSettings.SoundEnabled;
        public LanguageType Language => _persistentDataService.GameSettings.Language;

        public event EventHandler<SettingsChangedEventArgs> OnSettingsChanged;

        public SettingsService(IPersistentDataService persistentDataService)
        {
            _persistentDataService = persistentDataService;

            persistentDataService.RegisterDataReader(this);
        }

        public void ReadData(PlayerData playerData)
        {
            OnSettingsChanged?.Invoke(this, new SettingsChangedEventArgs(playerData.GameSettings));
        }

        public void MusicSetActive(bool activate)
        {
            _persistentDataService.GameSettings.MusicEnabled = activate;

            SaveAndInvokeSettingsChanged();
        }

        public void SoundSetActive(bool activate)
        {
            _persistentDataService.GameSettings.SoundEnabled = activate;

            SaveAndInvokeSettingsChanged();
        }

        public void SetLanguage(LanguageType language)
        {
            _persistentDataService.GameSettings.Language = language;

            SaveAndInvokeSettingsChanged();
        }

        private void SaveAndInvokeSettingsChanged()
        {
            _persistentDataService.Save();

            OnSettingsChanged?.Invoke(this, new SettingsChangedEventArgs(_persistentDataService.GameSettings));
        }
    }
}