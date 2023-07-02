using Data;

namespace Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ISaveLoadService _saveLoadService;

        private readonly GameSettings _gameSettings;

        public bool SoundEnabled => _gameSettings.SoundEnabled;

        public SettingsService(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;

            _gameSettings = _saveLoadService.LoadGameSettings() ?? CreateDefaultGameSettings();
        }

        public void SoundSetActive(bool activate)
        {
            _gameSettings.SoundEnabled = activate;
            _saveLoadService.SaveGameSettings(_gameSettings);
        }

        private static GameSettings CreateDefaultGameSettings() =>
            new GameSettings();
    }
}