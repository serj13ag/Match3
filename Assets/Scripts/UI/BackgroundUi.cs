using System.Collections;
using EventArgs;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class BackgroundUi : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;

        // TODO: separate classes
        [SerializeField] private TMP_Text _levelNameText;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _movesLeftText;

        private Coroutine _updateScoreRoutine;
        private int _currentScore;

        public void Init(ScoreService scoreService, CameraService cameraService, MovesLeftService movesLeftService)
        {
            _canvas.worldCamera = cameraService.MainCamera;

            UpdateSceneNameText();
            UpdateScoreText(scoreService.Score);
            UpdateMovesLeftText(movesLeftService.MovesLeft);

            scoreService.OnScoreChanged += OnScoreChanged;
            movesLeftService.OnMovesLeftChanged += OnMovesLeftChanged;
        }

        private void OnScoreChanged(object sender, ScoreChangedEventArgs e)
        {
            int oldScore = _currentScore;

            _currentScore = e.Score;

            _updateScoreRoutine ??= StartCoroutine(UpdateScoreTextRoutine(oldScore));
        }

        private void OnMovesLeftChanged(object sender, MovesLeftChangedEventArgs e)
        {
            UpdateMovesLeftText(e.MovesLeft);
        }

        private IEnumerator UpdateScoreTextRoutine(int oldScore)
        {
            int counterValue = oldScore;

            while (counterValue < _currentScore)
            {
                counterValue += Constants.Score.UpdateScoreTextIncrement;
                counterValue = Mathf.Min(counterValue, _currentScore);

                UpdateScoreText(counterValue);

                yield return new WaitForSeconds(Constants.Score.ScoreTextUpdateIntervalInSeconds);
            }

            UpdateScoreText(_currentScore);

            _updateScoreRoutine = null;
        }

        private void UpdateScoreText(int score)
        {
            _scoreText.text = score.ToString();
        }

        private void UpdateSceneNameText()
        {
            Scene scene = SceneManager.GetActiveScene();

            _levelNameText.text = scene.name;
        }

        private void UpdateMovesLeftText(int movesLeft)
        {
            _movesLeftText.text = movesLeft.ToString();
        }
    }
}