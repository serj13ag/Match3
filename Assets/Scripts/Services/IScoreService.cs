using System;
using EventArgs;

namespace Services
{
    public interface IScoreService
    {
        int Score { get; }
        bool ScoreGoalReached { get; }
        event EventHandler<ScoreChangedEventArgs> OnScoreChanged;

        void AddScore(int gamePieceScore, int numberOfBreakGamePieces,
            int completedBreakIterationsAfterSwitchedGamePieces);
    }
}