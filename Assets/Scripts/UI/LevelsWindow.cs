using EventArguments;
using Infrastructure.StateMachine;
using Services;
using Services.UI;
using StaticData;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelsWindow : MonoBehaviour
    {
        [SerializeField] private RectTransformMover _rectTransformMover;

        [SerializeField] private Transform _levelButtonsContainer;

        [SerializeField] private Button _backButton;

        private GameStateMachine _gameStateMachine;
        private IUiFactory _uiFactory;

        public void Init(GameStateMachine gameStateMachine, IUiFactory uiFactory, IStaticDataService staticDataService)
        {
            _uiFactory = uiFactory;
            _gameStateMachine = gameStateMachine;

            _backButton.onClick.AddListener(Back);

            foreach (LevelStaticData levelStaticData in staticDataService.Levels)
            {
                LevelButton levelButton = _uiFactory.GetLevelButton(_levelButtonsContainer);
                levelButton.Init(levelStaticData.LevelName);

                levelButton.OnButtonClicked += StartLevel;
            }

            _rectTransformMover.MoveIn();
        }

        private void StartLevel(object sender, LevelButtonClickedEventArgs e)
        {
            _gameStateMachine.Enter<GameLoopState, string>(e.LevelName);
        }

        private void Back()
        {
            _rectTransformMover.MoveOut();
        }
    }
}