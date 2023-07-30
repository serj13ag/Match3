using Enums;
using Services.UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _defaultModeButton;
        [SerializeField] private Button _puzzleModeButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _quitButton;

        private IWindowService _windowService;

        private void OnEnable()
        {
            _defaultModeButton.onClick.AddListener(StartDefaultMode);
            _puzzleModeButton.onClick.AddListener(ShowPuzzleLevels);
            _settingsButton.onClick.AddListener(ShowSettings);
            _quitButton.onClick.AddListener(QuitGame);
        }

        private void OnDisable()
        {
            _defaultModeButton.onClick.RemoveListener(StartDefaultMode);
            _puzzleModeButton.onClick.RemoveListener(ShowPuzzleLevels);
            _settingsButton.onClick.RemoveListener(ShowSettings);
            _quitButton.onClick.RemoveListener(QuitGame);
        }

        public void Init(IWindowService windowService)
        {
            _windowService = windowService;
        }

        private void StartDefaultMode()
        {
            throw new System.NotImplementedException();
        }

        private void ShowPuzzleLevels()
        {
            _windowService.ShowWindow(WindowType.PuzzleLevels);
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