using System.Collections.Generic;
using Enums;
using Newtonsoft.Json;
using UnityEngine;

namespace Services
{
    public class LocalizationService : ILocalizationService
    {
        private const string MissingTranslationString = "TRNSLMS";

        private readonly LanguageType _localizationType;
        private readonly Dictionary<string, string> _localization;

        public LocalizationService(IStaticDataService staticDataService, ISettingsService settingsService)
        {
            _localizationType = settingsService.Language;

            TextAsset localizationAsset = staticDataService.GetDataForLanguage(_localizationType);
            _localization = JsonConvert.DeserializeObject<Dictionary<string, string>>(localizationAsset.text);
        }

        public string GetTranslation(string key)
        {
            if (_localization.TryGetValue(key, out string translation))
            {
                return translation;
            }

            Debug.LogWarning($"Missing translation key {key} in {_localizationType}");
            return MissingTranslationString;
        }
    }
}