using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnityEngine;

namespace Services.Mono
{
    public class YaGamesMonoService : MonoBehaviour, IYaGamesMonoService
    {
        private Action<string> _onLoadedCallback;

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

        public void Save(string key, string jsonDataString)
        {
            SaveToPlayerData(key, jsonDataString);
        }

        public void Load(string key, Action<string> onLoadedCallback)
        {
            _onLoadedCallback = onLoadedCallback;
            LoadFromPlayerData(key);

            Debug.Log($"{nameof(YaGamesMonoService)}: Loading from player data");
        }

        // Call from YaAPI
        [UsedImplicitly]
        public void OnPlayerDataLoaded(string dataString)
        {
            Debug.Log($"{nameof(YaGamesMonoService)}: Call from YaAPI received, invoking {nameof(_onLoadedCallback)}");

            _onLoadedCallback?.Invoke(dataString);
            _onLoadedCallback = null;
        }

        // Call from YaAPI
        [UsedImplicitly]
        public void OnRewardedVideoWatched()
        {
        }
    }
}