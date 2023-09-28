using System;
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

        public void ShowFullAd(Action onFullAdWatchedCallback)
        {
            _yaGamesMonoService.ShowFullScreenAd(onFullAdWatchedCallback);
        }

        public void ShowRewardedAd(Action onRewardedAdWatchedCallback)
        {
            _yaGamesMonoService.ShowRewardedScreenAd(onRewardedAdWatchedCallback);
        }
    }
}