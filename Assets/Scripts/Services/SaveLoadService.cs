using Data;
using UnityEngine;

namespace Services
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string ProgressKey = "ProgressKey";
        private const string SettingsKey = "SettingsKey";

        public PlayerProgress LoadProgress()
        {
            string savedProgressString = PlayerPrefs.GetString(ProgressKey);
            return string.IsNullOrEmpty(savedProgressString)
                ? null
                : DeserializeJson<PlayerProgress>(savedProgressString);
        }

        public void SaveProgress(PlayerProgress progress) =>
            PlayerPrefs.SetString(ProgressKey, ToJson(progress));

        public GameSettings LoadGameSettings()
        {
            string savedProgressString = PlayerPrefs.GetString(SettingsKey);
            return string.IsNullOrEmpty(savedProgressString)
                ? null
                : DeserializeJson<GameSettings>(savedProgressString);
        }

        public void SaveGameSettings(GameSettings gameSettings) =>
            PlayerPrefs.SetString(SettingsKey, ToJson(gameSettings));

        private static string ToJson(object obj) =>
            JsonUtility.ToJson(obj);

        private static T DeserializeJson<T>(string json) =>
            JsonUtility.FromJson<T>(json);
    }
}