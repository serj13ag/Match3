using Constants;
using Data;
using Helpers;
using UnityEngine;

namespace Services
{
    public class LocalSaveService : ISaveService
    {
        public void SaveProgress(PlayerProgress progress)
        {
            PlayerPrefs.SetString(Settings.ProgressKey, JsonHelper.ToJson(progress));
        }

        public void SaveGameSettings(GameSettings gameSettings)
        {
            PlayerPrefs.SetString(Settings.SettingsKey, JsonHelper.ToJson(gameSettings));
        }
    }
}