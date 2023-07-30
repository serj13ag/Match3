using Infrastructure.StateMachine;
using Services;
using Services.MovesLeft;
using TMPro;
using UnityEngine;

namespace UI.Background
{
    public class PuzzleBackgroundScreen : BaseBackgroundScreen
    {
        [SerializeField] private ScoreCounter _scoreCounter;
        [SerializeField] private MovesLeftCounter _movesLeftCounter;

        [SerializeField] private TMP_Text _levelNameText;

        public void Init(string levelName, GameStateMachine gameStateMachine, IScoreService scoreService,
            ICameraService cameraService, IMovesLeftService movesLeftService)
        {
            InitInner(gameStateMachine, cameraService);

            _scoreCounter.Init(scoreService);
            _movesLeftCounter.Init(movesLeftService);

            UpdateSceneNameText(levelName);
        }

        private void UpdateSceneNameText(string levelName)
        {
            _levelNameText.text = levelName;
        }
    }
}