using Services;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Background
{
    public class BackgroundScreen : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;

        [SerializeField] private ScoreCounter _scoreCounter;
        [SerializeField] private MovesLeftCounter _movesLeftCounter;

        [SerializeField] private TMP_Text _levelNameText;

        private Coroutine _updateScoreRoutine;
        private int _currentScore;

        public void Init(IScoreService scoreService, ICameraService cameraService, IMovesLeftService movesLeftService)
        {
            _canvas.worldCamera = cameraService.MainCamera;

            _scoreCounter.Init(scoreService);
            _movesLeftCounter.Init(movesLeftService);

            UpdateSceneNameText();
        }

        private void UpdateSceneNameText()
        {
            Scene scene = SceneManager.GetActiveScene();

            _levelNameText.text = scene.name;
        }
    }
}