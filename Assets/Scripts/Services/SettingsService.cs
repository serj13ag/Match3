using System;
using Data;
using Enums;
using EventArguments;

namespace Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ISaveLoadService _saveLoadService;

        private readonly GameSettings _gameSettings;

        public bool MusicEnabled => _gameSettings.MusicEnabled;
        public bool SoundEnabled => _gameSettings.SoundEnabled;
        public LanguageType Language => _gameSettings.Language;

        public event EventHandler<SettingsChangedEventArgs> OnSettingsChanged;

        public SettingsService(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;

            _gameSettings = _saveLoadService.LoadGameSettings() ?? CreateDefaultGameSettings();
        }

        public void MusicSetActive(bool activate)
        {
            _gameSettings.MusicEnabled = activate;

            SaveAndInvokeSettingsChanged();
        }

        public void SoundSetActive(bool activate)
        {
            _gameSettings.SoundEnabled = activate;

            SaveAndInvokeSettingsChanged();
        }

        public void SetLanguage(LanguageType language)
        {
            _gameSettings.Language = language;

            SaveAndInvokeSettingsChanged();
        }

        private void SaveAndInvokeSettingsChanged()
        {
            _saveLoadService.SaveGameSettings(_gameSettings);

            OnSettingsChanged?.Invoke(this, new SettingsChangedEventArgs(_gameSettings));
        }

        private static GameSettings CreateDefaultGameSettings()
        {
            return new GameSettings();
        }
    }
}