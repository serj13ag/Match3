﻿using EventArguments;
using Infrastructure;
using Infrastructure.StateMachine;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class SettingsWindow : BaseWindow
    {
        private const float SoundButtonImageInactiveAlpha = 0.3f;

        [SerializeField] private Button _soundButton;
        [SerializeField] private Image _soundButtonImage;

        [SerializeField] private Button _resetButton;
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _backButton;

        private IPersistentProgressService _persistentProgressService;
        private ISettingsService _settingsService;

        private float _initialSoundButtonImageAlpha;
        private IGameStateMachine _gameStateMachine;

        private void OnEnable()
        {
            _soundButton.onClick.AddListener(SwitchSoundMode);
            _resetButton.onClick.AddListener(ResetProgressAndSave);
            _menuButton.onClick.AddListener(SwitchToMenu);
            _backButton.onClick.AddListener(Back);
        }

        private void OnDisable()
        {
            _soundButton.onClick.RemoveListener(SwitchSoundMode);
            _resetButton.onClick.RemoveListener(ResetProgressAndSave);
            _menuButton.onClick.RemoveListener(SwitchToMenu);
            _backButton.onClick.RemoveListener(Back);
        }

        private void Awake()
        {
            _gameStateMachine = ServiceLocator.Instance.Get<IGameStateMachine>();
            _settingsService = ServiceLocator.Instance.Get<ISettingsService>();
            _persistentProgressService = ServiceLocator.Instance.Get<IPersistentProgressService>();

            _initialSoundButtonImageAlpha = _soundButtonImage.color.a;

            UpdateSoundButtonColor(_settingsService.SoundEnabled);

            _menuButton.gameObject.SetActive(_gameStateMachine.InGameLoopState);

            _settingsService.OnSettingsChanged += OnSettingsChanged;
        }

        private void OnSettingsChanged(object sender, SettingsChangedEventArgs e)
        {
            UpdateSoundButtonColor(e.SoundEnabled);
        }

        private void SwitchSoundMode()
        {
            _settingsService.SoundSetActive(!_settingsService.SoundEnabled);
        }

        private void ResetProgressAndSave()
        {
            _persistentProgressService.ResetProgressAndSave();
        }

        private void SwitchToMenu()
        {
            _gameStateMachine.Enter<MainMenuState>();
        }

        private void Back()
        {
            Hide();
        }

        private void UpdateSoundButtonColor(bool soundEnabled)
        {
            Color newColor = _soundButtonImage.color;
            newColor.a = soundEnabled ? _initialSoundButtonImageAlpha : SoundButtonImageInactiveAlpha;
            _soundButtonImage.color = newColor;
        }

        private void OnDestroy()
        {
            _settingsService.OnSettingsChanged -= OnSettingsChanged;
        }
    }
}