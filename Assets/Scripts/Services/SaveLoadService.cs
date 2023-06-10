using Data;
using UnityEngine;

namespace Services
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string ProgressKey = "ProgressKey";

        private readonly IPersistentProgressService _persistentProgressService;

        public SaveLoadService(IPersistentProgressService persistentProgressService)
        {
            _persistentProgressService = persistentProgressService;
        }

        public PlayerProgress LoadProgress()
        {
            string savedProgressString = PlayerPrefs.GetString(ProgressKey);
            return string.IsNullOrEmpty(savedProgressString)
                ? null
                : DeserializeJson<PlayerProgress>(savedProgressString);
        }

        public void SaveProgress()
        {
            PlayerPrefs.SetString(ProgressKey, ToJson(_persistentProgressService.Progress));
        }

        private static string ToJson(object obj)
        {
            return JsonUtility.ToJson(obj);
        }

        private static T DeserializeJson<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }
    }
}