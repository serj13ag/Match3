namespace EventArgs
{
    public class ScoreChangedEventArgs : System.EventArgs
    {
        public int Score { get; }

        public ScoreChangedEventArgs(int score)
        {
            Score = score;
        }
    }
}