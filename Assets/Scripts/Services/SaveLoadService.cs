using Data;
using UnityEngine;

namespace Services
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string ProgressKey = "ProgressKey";

        public PlayerProgress LoadProgress()
        {
            string savedProgressString = PlayerPrefs.GetString(ProgressKey);
            return string.IsNullOrEmpty(savedProgressString)
                ? null
                : DeserializeJson<PlayerProgress>(savedProgressString);
        }

        public void SaveProgress(PlayerProgress progress)
        {
            PlayerPrefs.SetString(ProgressKey, ToJson(progress));
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