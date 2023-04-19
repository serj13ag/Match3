using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private const int UpdateScoreTextIncrement = 5;

    private const float ScoreTextUpdateIntervalInSeconds = 0.01f;
    private const int MinNumberOfBreakGamePiecesToGrantBonus = 4;
    private const int BonusScore = 20;

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
        var bonusScore = numberOfBreakGamePieces >= MinNumberOfBreakGamePiecesToGrantBonus
            ? BonusScore
            : 0;
        var scoreMultiplier = completedBreakIterationsAfterSwitchedGamePieces + 1;
        var totalScore = gamePieceScore * scoreMultiplier + bonusScore;

        AddScoreInner(totalScore);
    }

    private void AddScoreInner(int score)
    {
        var oldScore = _score;

        _score += score;

        _updateScoreRoutine ??= StartCoroutine(UpdateScoreTextRoutine(oldScore));
    }

    private IEnumerator UpdateScoreTextRoutine(int oldScore)
    {
        var counterValue = oldScore;

        while (counterValue < _score)
        {
            counterValue += UpdateScoreTextIncrement;
            counterValue = Mathf.Min(counterValue, _score);

            UpdateScoreText(counterValue);

            yield return new WaitForSeconds(ScoreTextUpdateIntervalInSeconds);
        }

        UpdateScoreText(_score);

        _updateScoreRoutine = null;
    }

    private void UpdateScoreText(int score)
    {
        _scoreText.text = score.ToString();
    }
}