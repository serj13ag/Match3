namespace EventArgs
{
    public class MovesLeftChangedEventArgs : System.EventArgs
    {
        public int MovesLeft { get; }

        public MovesLeftChangedEventArgs(int movesLeft)
        {
            MovesLeft = movesLeft;
        }
    }
}