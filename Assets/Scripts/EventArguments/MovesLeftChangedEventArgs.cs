using System;

namespace EventArguments
{
    public class MovesLeftChangedEventArgs : EventArgs
    {
        public int MovesLeft { get; }

        public MovesLeftChangedEventArgs(int movesLeft)
        {
            MovesLeft = movesLeft;
        }
    }
}