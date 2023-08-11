using System.Collections.Generic;
using EventArguments;
using Infrastructure;
using Infrastructure.StateMachine;
using Services;
using Services.UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class PuzzleLevelsWindow : BaseWindow
    {
        [SerializeField] private Transform _levelButtonsContainer;

        [SerializeField] private Button _backButton;

        private IGameStateMachine _gameStateMachine;
        private IUiFactory _uiFactory;

        private List<PuzzleLevelButton> _levelButtons;

        private void OnEnable()
        {
            _backButton.onClick.AddListener(Back);
        }

        private void OnDisable()
        {
            _backButton.onClick.RemoveListener(Back);
        }

        public void Init(IUiFactory uiFactory, IStaticDataService staticDataService)
        {
            _gameStateMachine = ServiceLocator.Instance.Get<IGameStateMachine>();

            _uiFactory = uiFactory;

            CreateLevelButtons(staticDataService);
        }

        private void CreateLevelButtons(IStaticDataService staticDataService)
        {
            _levelButtons = new List<PuzzleLevelButton>();

            foreach (string puzzleLevelName in staticDataService.PuzzleLevelNames)
            {
                PuzzleLevelButton puzzleLevelButton = _uiFactory.CreateLevelButton(_levelButtonsContainer);
                puzzleLevelButton.Init(puzzleLevelName);

                puzzleLevelButton.OnButtonClicked += StartPuzzleLevel;

                _levelButtons.Add(puzzleLevelButton);
            }
        }

        private void StartPuzzleLevel(object sender, PuzzleLevelButtonClickedEventArgs e)
        {
            Cleanup();
            _gameStateMachine.Enter<PuzzleGameLoopState, string>(e.LevelName);
        }

        private void Back()
        {
            Hide();
        }

        private void Cleanup()
        {
            foreach (PuzzleLevelButton levelButton in _levelButtons)
            {
                levelButton.OnButtonClicked -= StartPuzzleLevel;
            }

            _levelButtons.Clear();
        }
    }
}