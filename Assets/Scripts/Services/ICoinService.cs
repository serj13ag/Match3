using System;
using EventArguments;

namespace Services
{
    public interface ICoinService : IService
    {
        int Coins { get; }
        void IncrementCoins();
        event EventHandler<CoinsChangedEventArgs> OnCoinsChanged;
    }
}