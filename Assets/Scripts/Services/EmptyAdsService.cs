using System;
using UnityEngine;

namespace Services
{
    public class EmptyAdsService : IAdsService
    {
        public void ShowFullAd(Action onFullAdWatchedCallback)
        {
            Debug.Log($"{nameof(EmptyAdsService)}: {nameof(ShowFullAd)} call");

            onFullAdWatchedCallback?.Invoke();
        }

        public void ShowRewardedAd(Action onRewardedAdWatchedCallback)
        {
            onRewardedAdWatchedCallback?.Invoke();
        }
    }
}