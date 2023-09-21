using System;
using EventArguments;

namespace Services
{
    public interface ICoinService : IService
    {
        int Coins { get; }
        void IncrementCoins();
        void SpendCoins(int amount);
        event EventHandler<CoinsChangedEventArgs> OnCoinsChanged;
    }
}