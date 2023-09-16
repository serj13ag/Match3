using UnityEngine;

namespace Services
{
    public class EmptyAdsService : IAdsService
    {
        public void ShowFullAd()
        {
            Debug.Log($"{nameof(EmptyAdsService)}: {nameof(ShowFullAd)} call");
        }
    }
}