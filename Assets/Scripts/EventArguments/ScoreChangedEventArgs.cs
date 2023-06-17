using System;

namespace EventArguments
{
    public class ScoreChangedEventArgs : EventArgs
    {
        public int Score { get; }

        public ScoreChangedEventArgs(int score)
        {
            Score = score;
        }
    }
}