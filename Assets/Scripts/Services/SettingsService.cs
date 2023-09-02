using System;
using Data;
using EventArguments;

namespace Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ISaveLoadService _saveLoadService;

        private readonly GameSettings _gameSettings;

        public bool MusicEnabled => _gameSettings.MusicEnabled;
        public bool SoundEnabled => _gameSettings.SoundEnabled;

        public event EventHandler<SettingsChangedEventArgs> OnSettingsChanged;

        public SettingsService(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;

            _gameSettings = _saveLoadService.LoadGameSettings() ?? CreateDefaultGameSettings();
        }

        public void MusicSetActive(bool activate)
        {
            _gameSettings.MusicEnabled = activate;
            _saveLoadService.SaveGameSettings(_gameSettings);

            OnSettingsChanged?.Invoke(this, new SettingsChangedEventArgs(_gameSettings));
        }

        public void SoundSetActive(bool activate)
        {
            _gameSettings.SoundEnabled = activate;
            _saveLoadService.SaveGameSettings(_gameSettings);

            OnSettingsChanged?.Invoke(this, new SettingsChangedEventArgs(_gameSettings));
        }

        private static GameSettings CreateDefaultGameSettings() => new GameSettings();
    }
}