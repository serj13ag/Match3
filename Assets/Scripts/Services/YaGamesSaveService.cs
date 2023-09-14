using Constants;
using Data;
using Helpers;
using Services.Mono;

namespace Services
{
    public class YaGamesSaveService : ISaveService
    {
        private readonly IYaGamesMonoService _yaGamesMonoService;

        public YaGamesSaveService(IYaGamesMonoService yaGamesMonoService)
        {
            _yaGamesMonoService = yaGamesMonoService;
        }

        public void SaveProgress(PlayerProgress playerProgress)
        {
            string jsonProgressString = JsonHelper.ToJson(playerProgress);
            _yaGamesMonoService.Save(Settings.ProgressKey, jsonProgressString);
        }

        public void SaveGameSettings(GameSettings gameSettings)
        {
            string jsonSettingsString = JsonHelper.ToJson(gameSettings);
            _yaGamesMonoService.Save(Settings.SettingsKey, jsonSettingsString);
        }
    }
}