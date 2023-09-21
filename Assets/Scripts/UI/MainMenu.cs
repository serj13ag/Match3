using Enums;
using Infrastructure.StateMachine;
using Services.UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _defaultModeButton;
        [SerializeField] private Button _puzzleModeButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _quitButton;

        private IWindowService _windowService;
        private IGameStateMachine _gameStateMachine;

        private void OnEnable()
        {
            _defaultModeButton.onClick.AddListener(StartDefaultMode);
            _puzzleModeButton.onClick.AddListener(ShowPuzzleLevels);
            _shopButton.onClick.AddListener(ShowShop);
            _settingsButton.onClick.AddListener(ShowSettings);
            _quitButton.onClick.AddListener(QuitGame);
        }

        private void OnDisable()
        {
            _defaultModeButton.onClick.RemoveListener(StartDefaultMode);
            _puzzleModeButton.onClick.RemoveListener(ShowPuzzleLevels);
            _shopButton.onClick.RemoveListener(ShowShop);
            _settingsButton.onClick.RemoveListener(ShowSettings);
            _quitButton.onClick.RemoveListener(QuitGame);
        }

        public void Init(IGameStateMachine gameStateMachine, IWindowService windowService)
        {
            _gameStateMachine = gameStateMachine;
            _windowService = windowService;
        }

        private void StartDefaultMode()
        {
            _gameStateMachine.Enter<EndlessGameLoopState>();
        }

        private void ShowPuzzleLevels()
        {
            _windowService.ShowWindow(WindowType.PuzzleLevels);
        }

        private void ShowShop()
        {
            _windowService.ShowWindow(WindowType.Shop);
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