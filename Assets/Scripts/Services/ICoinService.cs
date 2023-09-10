using System;
using EventArguments;

namespace Services
{
    public interface ICoinService : IService
    {
        int Coins { get; }
        void UpdateCoinsFromProgress();
        void IncrementCoins();
        event EventHandler<CoinsChangedEventArgs> OnCoinsChanged;
    }
}