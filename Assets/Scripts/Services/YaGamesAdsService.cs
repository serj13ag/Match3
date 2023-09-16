using Services.Mono;

namespace Services
{
    public class YaGamesAdsService : IAdsService
    {
        private readonly IYaGamesMonoService _yaGamesMonoService;

        public YaGamesAdsService(IYaGamesMonoService yaGamesMonoService)
        {
            _yaGamesMonoService = yaGamesMonoService;
        }

        public void ShowFullAd()
        {
            _yaGamesMonoService.ShowFullScreenAd();
        }
    }
}