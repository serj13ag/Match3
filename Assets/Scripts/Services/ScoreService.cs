using System;
using EventArgs;

namespace Services
{
    public class ScoreService
    {
        private int _score;

        public int Score
        {
            get => _score;
            private set
            {
                if (_score != value)
                {
                    _score = value;

                    OnScoreChanged?.Invoke(this, new ScoreChangedEventArgs(_score));
                }
            }
        }

        public event EventHandler<ScoreChangedEventArgs> OnScoreChanged;

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
    }
}