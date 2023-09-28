using System;

namespace Services
{
    public interface IAdsService : IService
    {
        void ShowFullAd(Action onFullAdWatchedCallback);
        void ShowRewardedAd(Action onRewardedAdWatchedCallback);
    }
}