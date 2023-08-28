using System;
using EventArguments;

namespace Services
{
    public class CoinService : ICoinService
    {
        private readonly IPersistentProgressService _persistentProgressService;

        private int _coins;

        public int Coins
        {
            get => _coins;
            private set
            {
                if (value != _coins)
                {
                    _coins = value;
                    OnCoinsChanged?.Invoke(this, new CoinsChangedEventArgs(_coins));
                }
            }
        }

        public event EventHandler<CoinsChangedEventArgs> OnCoinsChanged;

        public CoinService(IPersistentProgressService persistentProgressService)
        {
            _persistentProgressService = persistentProgressService;

            _coins = persistentProgressService.Progress.Coins;
        }

        public void IncrementCoins()
        {
            Coins++;

            UpdateAndSaveProgress();
        }

        private void UpdateAndSaveProgress()
        {
            _persistentProgressService.Progress.Coins = _coins;
            _persistentProgressService.SaveProgress();
        }
    }
}