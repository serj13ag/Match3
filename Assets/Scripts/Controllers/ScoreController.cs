using System.Collections;
using TMPro;
using UnityEngine;

namespace Controllers
{
    public class ScoreController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;

        private int _score;
        private Coroutine _updateScoreRoutine;

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

            AddScoreInner(totalScore);
        }

        private void AddScoreInner(int score)
        {
            int oldScore = _score;

            _score += score;

            _updateScoreRoutine ??= StartCoroutine(UpdateScoreTextRoutine(oldScore));
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
            _scoreText.text = score.ToString();
        }
    }
}