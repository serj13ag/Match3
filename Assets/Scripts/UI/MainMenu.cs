using Infrastructure.StateMachine;
using Services;
using Services.UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _quitButton;

        private GameStateMachine _gameStateMachine;
        private IUiFactory _uiFactory;
        private IStaticDataService _staticDataService;

        public void Init(GameStateMachine gameStateMachine, IUiFactory uiFactory, IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
            _uiFactory = uiFactory;
            _gameStateMachine = gameStateMachine;

            _playButton.onClick.AddListener(OnPlayButtonClick);
            _quitButton.onClick.AddListener(QuitGame);
        }

        private void OnPlayButtonClick()
        {
            LevelsWindow levelsWindow = _uiFactory.GetLevelsWindow();
            levelsWindow.Init(_gameStateMachine, _uiFactory, _staticDataService);
        }

        private void QuitGame()
        {
            Application.Quit();
        }

        private void OnDestroy()
        {
            _playButton.onClick.RemoveListener(OnPlayButtonClick);
            _quitButton.onClick.RemoveListener(QuitGame);
        }
    }
}