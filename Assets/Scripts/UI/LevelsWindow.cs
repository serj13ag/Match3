using System.Collections.Generic;
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

        private List<LevelButton> _levelButtons;

        private void OnEnable()
        {
            _backButton.onClick.AddListener(Back);
        }

        private void OnDisable()
        {
            _backButton.onClick.RemoveListener(Back);
        }

        public void Init(GameStateMachine gameStateMachine, IUiFactory uiFactory, IStaticDataService staticDataService)
        {
            _uiFactory = uiFactory;
            _gameStateMachine = gameStateMachine;

            CreateLevelButtons(staticDataService);
        }

        public void Show()
        {
            _rectTransformMover.MoveIn();
        }

        private void CreateLevelButtons(IStaticDataService staticDataService)
        {
            _levelButtons = new List<LevelButton>();

            foreach (LevelStaticData levelStaticData in staticDataService.Levels)
            {
                LevelButton levelButton = _uiFactory.CreateLevelButton(_levelButtonsContainer);
                levelButton.Init(levelStaticData.LevelName);

                levelButton.OnButtonClicked += StartLevel;

                _levelButtons.Add(levelButton);
            }
        }

        private void StartLevel(object sender, LevelButtonClickedEventArgs e)
        {
            Cleanup();
            _gameStateMachine.Enter<GameLoopState, string>(e.LevelName);
        }

        private void Back()
        {
            _rectTransformMover.MoveOut();
        }

        private void Cleanup()
        {
            foreach (LevelButton levelButton in _levelButtons)
            {
                levelButton.OnButtonClicked -= StartLevel;
            }

            _levelButtons.Clear();
        }
    }
}