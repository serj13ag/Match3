using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnityEngine;

namespace Services.Mono
{
    public class YaGamesMonoService : MonoBehaviour, IYaGamesMonoService
    {
        private Action _onSDKInitCallback;
        private Action<string> _onPlayerDataLoadedCallback;
        private Action _onFullAdWatchedCallback;
        private Action _onRewardedAdWatchedCallback;

        [DllImport("__Internal")]
        private static extern void InitSDK();

        [DllImport("__Internal")]
        private static extern void GameReady();

        [DllImport("__Internal")]
        private static extern void SaveToPlayerData(string key, string jsonDataString);

        [DllImport("__Internal")]
        private static extern void LoadFromPlayerData(string key);

        [DllImport("__Internal")]
        private static extern void ShowFullAd();

        [DllImport("__Internal")]
        private static extern void ShowRewardedAd();

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void InitYaSDK(Action onSDKInitCallback)
        {
            InitSDK();
            _onSDKInitCallback = onSDKInitCallback;
        }

        public void GameReadyNotify()
        {
            GameReady();
        }

        public void Save(string key, string jsonDataString)
        {
            SaveToPlayerData(key, jsonDataString);
        }

        public void Load(string key, Action<string> onPlayerDataLoadedCallback)
        {
            _onPlayerDataLoadedCallback = onPlayerDataLoadedCallback;
            LoadFromPlayerData(key);

            Debug.Log($"{nameof(YaGamesMonoService)}: Loading from player data");
        }

        public void ShowFullScreenAd(Action onFullAdWatchedCallback)
        {
            _onFullAdWatchedCallback = onFullAdWatchedCallback;
            ShowFullAd();

            Debug.Log($"{nameof(YaGamesMonoService)}: Showing full ad");
        }

        public void ShowRewardedScreenAd(Action onRewardedAdWatchedCallback)
        {
            _onRewardedAdWatchedCallback = onRewardedAdWatchedCallback;
            ShowRewardedAd();
        }

        // Call from YaAPI
        [UsedImplicitly]
        public void OnSDKInitCompleted()
        {
            Debug.Log($"{nameof(YaGamesMonoService)}: Call from YaAPI received, invoking {nameof(_onSDKInitCallback)}");

            _onSDKInitCallback?.Invoke();
            _onSDKInitCallback = null;
        }

        // Call from YaAPI
        [UsedImplicitly]
        public void OnPlayerDataLoaded(string dataString)
        {
            Debug.Log($"{nameof(YaGamesMonoService)}: Call from YaAPI received, invoking {nameof(_onPlayerDataLoadedCallback)}");

            _onPlayerDataLoadedCallback?.Invoke(dataString);
            _onPlayerDataLoadedCallback = null;
        }

        // Call from YaAPI
        [UsedImplicitly]
        public void OnRewardedAdWatched()
        {
            Debug.Log($"{nameof(YaGamesMonoService)}: Call from YaAPI received, invoking {nameof(_onRewardedAdWatchedCallback)}");

            _onRewardedAdWatchedCallback?.Invoke();
            _onRewardedAdWatchedCallback = null;
        }
        
        // Call from YaAPI
        [UsedImplicitly]
        public void OnFullAdWatched()
        {
            Debug.Log($"{nameof(YaGamesMonoService)}: Call from YaAPI received, invoking {nameof(_onFullAdWatchedCallback)}");

            _onFullAdWatchedCallback?.Invoke();
            _onFullAdWatchedCallback = null;
        }
    }
}