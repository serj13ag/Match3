using Data;
using Services.PersistentProgress;
using UnityEngine;

namespace Services
{
    public class SaveLoadService
    {
        private const string ProgressKey = "ProgressKey";

        private readonly PersistentProgressService _persistentProgressService;

        public SaveLoadService(PersistentProgressService persistentProgressService)
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