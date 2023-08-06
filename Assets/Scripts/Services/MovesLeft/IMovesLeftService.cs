using System;
using EventArguments;

namespace Services.MovesLeft
{
    public interface IMovesLeftService
    {
        int MovesLeft { get; }
        event EventHandler<MovesLeftChangedEventArgs> OnMovesLeftChanged;
        void DecrementMovesLeft();
    }
}