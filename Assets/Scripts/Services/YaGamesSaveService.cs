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

        public void SaveData(PlayerData playerData)
        {
            string jsonPlayerDataString = JsonHelper.ToJson(playerData);
            _yaGamesMonoService.Save(Settings.SavedPlayerDataKey, jsonPlayerDataString);
        }
    }
}