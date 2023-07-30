using System;
using EventArguments;

namespace Services.MovesLeft
{
    public class InfiniteMovesLeftService : IMovesLeftService
    {
        public int MovesLeft => int.MaxValue;

        public event EventHandler<MovesLeftChangedEventArgs> OnMovesLeftChanged;

        public void DecrementMovesLeft()
        {
        }
    }
}