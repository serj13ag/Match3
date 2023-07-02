using EventArguments;
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
        [SerializeField] private Button _backButton;

        private IPersistentProgressService _persistentProgressService;
        private ISettingsService _settingsService;

        private float _initialSoundButtonImageAlpha;

        private void OnEnable()
        {
            _soundButton.onClick.AddListener(SwitchSoundMode);
            _resetButton.onClick.AddListener(ResetProgressAndSave);
            _backButton.onClick.AddListener(Back);
        }

        private void OnDisable()
        {
            _soundButton.onClick.RemoveListener(SwitchSoundMode);
            _resetButton.onClick.RemoveListener(ResetProgressAndSave);
            _backButton.onClick.RemoveListener(Back);
        }

        public void Init(IPersistentProgressService persistentProgressService, ISettingsService settingsService)
        {
            _settingsService = settingsService;
            _persistentProgressService = persistentProgressService;

            _initialSoundButtonImageAlpha = _soundButtonImage.color.a;

            UpdateSoundButtonColor(_settingsService.SoundEnabled);

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
    }
}