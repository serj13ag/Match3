using System;

namespace Services
{
    public interface IAdsService : IService
    {
        void ShowFullAd();
        void ShowRewardedAd(Action onRewardedAdWatchedCallback);
    }
}