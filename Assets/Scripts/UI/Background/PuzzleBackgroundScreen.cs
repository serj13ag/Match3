using Services;
using Services.MovesLeft;
using Services.UI;
using TMPro;
using UnityEngine;

namespace UI.Background
{
    public class PuzzleBackgroundScreen : BaseBackgroundScreen
    {
        [SerializeField] private ScoreCounter _scoreCounter;
        [SerializeField] private MovesLeftCounter _movesLeftCounter;

        [SerializeField] private TMP_Text _levelNameText;

        public void Init(string levelName, IScoreService scoreService, ICameraService cameraService,
            IMovesLeftService movesLeftService, IWindowService windowService)
        {
            InitInner(cameraService, windowService);

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