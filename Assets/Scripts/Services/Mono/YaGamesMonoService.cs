using System.Runtime.InteropServices;
using Data;
using UnityEngine;
using Newtonsoft.Json;

namespace Services.Mono
{
    public class YaGamesMonoService : MonoBehaviour, IYaGamesMonoService
    {
        [DllImport("__Internal")]
        private static extern void InitYandexSDK();

        [DllImport("__Internal")]
        private static extern void SaveToPlayerData(string key, string jsonDataString);

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        [DllImport("__Internal")]
        private static extern void LoadFromPlayerData(string key);

        public void OnPlayerDataLoaded(string dataString)
        {
            JsonConvert.DeserializeObject<PlayerProgress>(dataString);
        }
    }
}