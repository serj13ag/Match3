using System;
using Data;
using Enums;
using EventArguments;

namespace Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ISaveService _saveService;

        private GameSettings _gameSettings;

        public bool MusicEnabled => _gameSettings.MusicEnabled;
        public bool SoundEnabled => _gameSettings.SoundEnabled;
        public LanguageType Language => _gameSettings.Language;

        public event EventHandler<SettingsChangedEventArgs> OnSettingsChanged;

        public SettingsService(ISaveService saveService)
        {
            _saveService = saveService;
        }

        public void InitGameSettings(GameSettings gameSettings)
        {
            _gameSettings = gameSettings ?? CreateDefaultGameSettings();

            OnSettingsChanged?.Invoke(this, new SettingsChangedEventArgs(_gameSettings));
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
            _saveService.SaveGameSettings(_gameSettings);

            OnSettingsChanged?.Invoke(this, new SettingsChangedEventArgs(_gameSettings));
        }

        private static GameSettings CreateDefaultGameSettings()
        {
            return new GameSettings();
        }
    }
}