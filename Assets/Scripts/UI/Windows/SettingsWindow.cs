using EventArguments;
using Infrastructure;
using Infrastructure.StateMachine;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class SettingsWindow : BaseWindow
    {
        [SerializeField] private ToggleButton _musicButton;
        [SerializeField] private ToggleButton _soundsButton;

        [SerializeField] private Button _resetButton;
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _backButton;

        private IPersistentProgressService _persistentProgressService;
        private ISettingsService _settingsService;

        private IGameStateMachine _gameStateMachine;

        private void OnEnable()
        {
            _musicButton.Button.onClick.AddListener(SwitchMusicMode);
            _soundsButton.Button.onClick.AddListener(SwitchSoundMode);
            _resetButton.onClick.AddListener(ResetProgressAndSave);
            _menuButton.onClick.AddListener(SwitchToMenu);
            _backButton.onClick.AddListener(Back);
        }

        private void OnDisable()
        {
            _musicButton.Button.onClick.RemoveListener(SwitchMusicMode);
            _soundsButton.Button.onClick.RemoveListener(SwitchSoundMode);
            _resetButton.onClick.RemoveListener(ResetProgressAndSave);
            _menuButton.onClick.RemoveListener(SwitchToMenu);
            _backButton.onClick.RemoveListener(Back);
        }

        private void Awake()
        {
            _gameStateMachine = ServiceLocator.Instance.Get<IGameStateMachine>();
            _settingsService = ServiceLocator.Instance.Get<ISettingsService>();
            _persistentProgressService = ServiceLocator.Instance.Get<IPersistentProgressService>();

            _musicButton.Init(_settingsService.MusicEnabled);
            _soundsButton.Init(_settingsService.SoundEnabled);

            _menuButton.gameObject.SetActive(_gameStateMachine.InGameLoopState);

            _settingsService.OnSettingsChanged += OnSettingsChanged;
        }

        private void OnSettingsChanged(object sender, SettingsChangedEventArgs e)
        {
            _musicButton.UpdateSoundButtonColor(e.MusicEnabled);
            _soundsButton.UpdateSoundButtonColor(e.SoundEnabled);
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

        private void OnDestroy()
        {
            _settingsService.OnSettingsChanged -= OnSettingsChanged;
        }
    }
}