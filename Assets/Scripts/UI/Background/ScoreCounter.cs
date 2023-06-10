using System.Collections;
using Constants;
using EventArgs;
using Services;
using TMPro;
using UnityEngine;

namespace UI.Background
{
    public class ScoreCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;

        private int _currentScore;
        private Coroutine _updateScoreRoutine;

        public void Init(IScoreService scoreService)
        {
            UpdateScoreText(scoreService.Score);

            scoreService.OnScoreChanged += OnScoreChanged;
        }

        private void OnScoreChanged(object sender, ScoreChangedEventArgs e)
        {
            int oldScore = _currentScore;

            _currentScore = e.Score;

            _updateScoreRoutine ??= StartCoroutine(UpdateScoreTextRoutine(oldScore));
        }

        private IEnumerator UpdateScoreTextRoutine(int oldScore)
        {
            int counterValue = oldScore;

            while (counterValue < _currentScore)
            {
                counterValue += Settings.Score.UpdateScoreTextIncrement;
                counterValue = Mathf.Min(counterValue, _currentScore);

                UpdateScoreText(counterValue);

                yield return new WaitForSeconds(Settings.Score.ScoreTextUpdateIntervalInSeconds);
            }

            UpdateScoreText(_currentScore);

            _updateScoreRoutine = null;
        }

        private void UpdateScoreText(int score)
        {
            _scoreText.text = score.ToString();
        }
    }
}