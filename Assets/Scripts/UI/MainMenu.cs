using Enums;
using Services.UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _quitButton;

        private IWindowService _windowService;

        private void OnEnable()
        {
            _playButton.onClick.AddListener(ShowLevels);
            _settingsButton.onClick.AddListener(ShowSettings);
            _quitButton.onClick.AddListener(QuitGame);
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveListener(ShowLevels);
            _settingsButton.onClick.RemoveListener(ShowSettings);
            _quitButton.onClick.RemoveListener(QuitGame);
        }

        public void Init(IWindowService windowService)
        {
            _windowService = windowService;
        }

        private void ShowLevels()
        {
            _windowService.ShowWindow(WindowType.Levels);
        }

        private void ShowSettings()
        {
            _windowService.ShowWindow(WindowType.Settings);
        }

        private void QuitGame()
        {
            Application.Quit();
        }
    }
}