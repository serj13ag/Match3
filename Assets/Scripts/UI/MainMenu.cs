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

        private IUiFactory _uiFactory;

        public void Init(IUiFactory uiFactory)
        {
            _uiFactory = uiFactory;

            _playButton.onClick.AddListener(ShowLevels);
            _settingsButton.onClick.AddListener(ShowSettings);
            _quitButton.onClick.AddListener(QuitGame);
        }

        private void ShowLevels()
        {
            LevelsWindow levelsWindow = _uiFactory.CreateLevelsWindow();
            levelsWindow.Show();
        }

        private void ShowSettings()
        {
            SettingsWindow settingsWindow = _uiFactory.CreateSettingsWindow();
            settingsWindow.Show();
        }

        private void QuitGame()
        {
            Application.Quit();
        }

        private void OnDestroy()
        {
            _playButton.onClick.RemoveListener(ShowLevels);
            _settingsButton.onClick.RemoveListener(ShowSettings);
            _quitButton.onClick.RemoveListener(QuitGame);
        }
    }
}