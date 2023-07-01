using System.Collections;
using Constants;
using EventArguments;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Background
{
    public class ScoreCounter : MonoBehaviour
    {
        [SerializeField] private Image _circleImage;
        [SerializeField] private TMP_Text _scoreText;

        private IScoreService _scoreService;

        private int _currentScore;
        private Coroutine _updateScoreViewRoutine;

        public void Init(IScoreService scoreService)
        {
            _scoreService = scoreService;

            _currentScore = _scoreService.Score;
            UpdateView(scoreService.Score);

            scoreService.OnScoreChanged += OnScoreChanged;
        }

        private void OnScoreChanged(object sender, ScoreChangedEventArgs e)
        {
            int oldScore = _currentScore;

            _currentScore = e.Score;

            _updateScoreViewRoutine ??= StartCoroutine(UpdateScoreViewRoutine(oldScore));
        }

        private IEnumerator UpdateScoreViewRoutine(int oldScore)
        {
            int counterValue = oldScore;

            while (counterValue < _currentScore)
            {
                counterValue += Settings.Score.UpdateScoreTextIncrement;
                counterValue = Mathf.Min(counterValue, _currentScore);

                UpdateView(counterValue);

                yield return new WaitForSeconds(Settings.Score.ScoreTextUpdateIntervalInSeconds);
            }

            UpdateView(_currentScore);

            _updateScoreViewRoutine = null;
        }

        private void UpdateView(int score)
        {
            UpdateScoreText(score);
            UpdateImageFill(score);
        }

        private void UpdateScoreText(int score)
        {
            _scoreText.text = score.ToString();
        }

        private void UpdateImageFill(int score)
        {
            _circleImage.fillAmount = (float)score / _scoreService.ScoreGoal;
        }
    }
}