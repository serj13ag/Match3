using Data;
using UnityEngine;

namespace Services
{
    public class SaveLoadService
    {
        private const string ProgressKey = "ProgressKey";

        public PlayerProgress LoadProgress()
        {
            string savedProgressString = PlayerPrefs.GetString(ProgressKey);
            return string.IsNullOrEmpty(savedProgressString)
                ? null
                : DeserializeJson<PlayerProgress>(savedProgressString);
        }

        public void SaveProgress()
        {
        }

        private static T DeserializeJson<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }
    }
}