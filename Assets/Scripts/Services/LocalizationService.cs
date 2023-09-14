using System;
using System.Collections.Generic;
using Enums;
using EventArguments;
using Helpers;
using Newtonsoft.Json;
using UnityEngine;

namespace Services
{
    public class LocalizationService : ILocalizationService
    {
        private const string MissingTranslationString = "TRNSLMS";

        private readonly IStaticDataService _staticDataService;

        private LanguageType _currentLocalizationType;
        private Dictionary<string, string> _currentLocalization;

        public event EventHandler<EventArgs> LocalizationChanged;

        public LocalizationService(IStaticDataService staticDataService, ISettingsService settingsService)
        {
            _staticDataService = staticDataService;

            settingsService.OnSettingsChanged += OnSettingsChanged;
        }

        public string GetTranslation(string key)
        {
            if (_currentLocalization.TryGetValue(key, out string translation))
            {
                return translation;
            }

            Debug.LogWarning($"Missing translation key {key} in {_currentLocalizationType}");
            return MissingTranslationString;
        }

        private void OnSettingsChanged(object sender, SettingsChangedEventArgs e)
        {
            SetLocalization(e.GameSettings.Language);

            LocalizationChanged?.Invoke(this, EventArgs.Empty);
        }

        private void SetLocalization(LanguageType languageType)
        {
            _currentLocalizationType = languageType;

            TextAsset localizationAsset = _staticDataService.GetDataForLanguage(_currentLocalizationType).Translations;
            _currentLocalization =JsonHelper.FromJson<Dictionary<string, string>>(localizationAsset.text);
        }
    }
}