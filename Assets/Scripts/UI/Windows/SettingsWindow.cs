using System.Collections.Generic;
using Enums;
using EventArguments;
using Infrastructure;
using Infrastructure.StateMachine;
using Services;
using StaticData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class SettingsWindow : BaseWindow
    {
        [SerializeField] private TMP_Dropdown _languagesDropdown;

        [SerializeField] private ToggleButton _musicButton;
        [SerializeField] private ToggleButton _soundsButton;

        [SerializeField] private Button _resetButton;
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _backButton;

        private IPersistentDataService _persistentDataService;
        private ISettingsService _settingsService;
        private IStaticDataService _staticDataService;
        private IGameStateMachine _gameStateMachine;

        private void OnEnable()
        {
            _musicButton.Button.onClick.AddListener(SwitchMusicMode);
            _soundsButton.Button.onClick.AddListener(SwitchSoundMode);
            _resetButton.onClick.AddListener(ResetProgressAndSave);
            _menuButton.onClick.AddListener(SwitchToMenu);
            _backButton.onClick.AddListener(Back);

            _languagesDropdown.onValueChanged.AddListener(ChangeLanguage);
        }

        private void OnDisable()
        {
            _musicButton.Button.onClick.RemoveListener(SwitchMusicMode);
            _soundsButton.Button.onClick.RemoveListener(SwitchSoundMode);
            _resetButton.onClick.RemoveListener(ResetProgressAndSave);
            _menuButton.onClick.RemoveListener(SwitchToMenu);
            _backButton.onClick.RemoveListener(Back);

            _languagesDropdown.onValueChanged.RemoveListener(ChangeLanguage);
        }

        private void Awake()
        {
            _gameStateMachine = ServiceLocator.Instance.Get<IGameStateMachine>();
            _settingsService = ServiceLocator.Instance.Get<ISettingsService>();
            _persistentDataService = ServiceLocator.Instance.Get<IPersistentDataService>();
            _staticDataService = ServiceLocator.Instance.Get<IStaticDataService>();

            InitLanguagesDropdown();

            _musicButton.Init(_settingsService.MusicEnabled);
            _soundsButton.Init(_settingsService.SoundEnabled);

            _menuButton.gameObject.SetActive(_gameStateMachine.InGameLoopState);

            _settingsService.OnSettingsChanged += OnSettingsChanged;
        }

        private void InitLanguagesDropdown()
        {
            List<LanguageType> languages = _staticDataService.AvailableLanguages;
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>(languages.Count);

            for (int i = 0; i < languages.Count; i++)
            {
                LanguageStaticData languageData = _staticDataService.GetDataForLanguage(languages[i]);
                options.Add(new TMP_Dropdown.OptionData(languageData.NameString));
            }

            _languagesDropdown.options = options;
            _languagesDropdown.value = languages.IndexOf(_settingsService.Language);
        }

        private void OnSettingsChanged(object sender, SettingsChangedEventArgs e)
        {
            _musicButton.UpdateSoundButtonColor(e.GameSettings.MusicEnabled);
            _soundsButton.UpdateSoundButtonColor(e.GameSettings.SoundEnabled);
        }

        private void ChangeLanguage(int arg)
        {
            _settingsService.SetLanguage(_staticDataService.AvailableLanguages[arg]);
        }

        private void SwitchMusicMode()
        {
            _settingsService.MusicSetActive(!_settingsService.MusicEnabled);
        }

        private void SwitchSoundMode()
        {
            _settingsService.SoundSetActive(!_settingsService.SoundEnabled);
        }

        private void ResetProgressAndSave()
        {
            _persistentDataService.ResetProgressAndSave();
        }

        private void SwitchToMenu()
        {
            _gameStateMachine.Enter<MainMenuState>();
        }

        private void Back()
        {
            Hide();
        }

        private void OnDestroy()
        {
            _settingsService.OnSettingsChanged -= OnSettingsChanged;
        }
    }
}