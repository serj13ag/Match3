using System;
using System.Collections;
using UnityEngine;

namespace Services.Mono
{
    public class ScoreController : MonoBehaviour
    {
        private int _score;
        private Coroutine _updateScoreRoutine;

        public int Score
        {
            get => _score;
            private set
            {
                if (_score != value)
                {
                    int oldScore = Score;

                    _score = value;

                    _updateScoreRoutine ??= StartCoroutine(UpdateScoreTextRoutine(oldScore));

                    OnScoreChanged?.Invoke();
                }
            }
        }

        public event Action OnScoreChanged;

        private void Awake()
        {
            UpdateScoreText(0);
        }

        public void AddScore(int gamePieceScore, int numberOfBreakGamePieces,
            int completedBreakIterationsAfterSwitchedGamePieces)
        {
            int bonusScore = numberOfBreakGamePieces >= Constants.Score.MinNumberOfBreakGamePiecesToGrantBonus
                ? Constants.Score.BonusScore
                : 0;
            int scoreMultiplier = completedBreakIterationsAfterSwitchedGamePieces + 1;
            int totalScore = gamePieceScore * scoreMultiplier + bonusScore;

            Score += totalScore;
        }

        private IEnumerator UpdateScoreTextRoutine(int oldScore)
        {
            int counterValue = oldScore;

            while (counterValue < _score)
            {
                counterValue += Constants.Score.UpdateScoreTextIncrement;
                counterValue = Mathf.Min(counterValue, _score);

                UpdateScoreText(counterValue);

                yield return new WaitForSeconds(Constants.Score.ScoreTextUpdateIntervalInSeconds);
            }

            UpdateScoreText(_score);

            _updateScoreRoutine = null;
        }

        private void UpdateScoreText(int score)
        {
            //_scoreText.text = score.ToString(); // TODO
        }
    }
}