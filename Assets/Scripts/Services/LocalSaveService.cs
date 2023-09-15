using Constants;
using Data;
using Helpers;
using UnityEngine;

namespace Services
{
    public class LocalSaveService : ISaveService
    {
        public void SaveData(PlayerData playerData)
        {
            PlayerPrefs.SetString(Settings.SavedPlayerDataKey, JsonHelper.ToJson(playerData));
        }
    }
}