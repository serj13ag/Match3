using System;

namespace Services.Mono
{
    public class NullYaGamesMonoService : IYaGamesMonoService
    {
        public void Save(string key, string jsonDataString) { }
        public void Load(string key, Action<string> onPlayerDataLoadedCallback) { }
        public void ShowFullScreenAd(Action onFullAdWatchedCallback) { }
        public void ShowRewardedScreenAd(Action onRewardedAdWatchedCallback) { }
        public void InitYaSDK(Action onSDKInitCallback) { }
        public void GameReadyNotify() { }
    }
}