using System;
using EventArgs;

namespace Services
{
    public interface IMovesLeftService
    {
        int MovesLeft { get; }
        event EventHandler<MovesLeftChangedEventArgs> OnMovesLeftChanged;
    }
}