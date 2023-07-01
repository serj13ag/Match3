using Infrastructure.StateMachine;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Background
{
    public class BackgroundScreen : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;

        [SerializeField] private ScoreCounter _scoreCounter;
        [SerializeField] private MovesLeftCounter _movesLeftCounter;

        [SerializeField] private TMP_Text _levelNameText;

        [SerializeField] private Button _menuButton;

        private GameStateMachine _gameStateMachine;

        private Coroutine _updateScoreRoutine;
        private int _currentScore;

        public void Init(string levelName, GameStateMachine gameStateMachine, IScoreService scoreService,
            ICameraService cameraService, IMovesLeftService movesLeftService)
        {
            _gameStateMachine = gameStateMachine;
            _canvas.worldCamera = cameraService.MainCamera;

            _scoreCounter.Init(scoreService);
            _movesLeftCounter.Init(movesLeftService);

            UpdateSceneNameText(levelName);

            _menuButton.onClick.AddListener(GoToMainMenu);
        }

        private void UpdateSceneNameText(string levelName)
        {
            _levelNameText.text = levelName;
        }

        private void GoToMainMenu()
        {
            _gameStateMachine.Enter<MainMenuState>();
        }
    }
}