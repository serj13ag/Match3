using System;
using EventArguments;

namespace Services
{
    public class CoinService : ICoinService
    {
        private readonly IPersistentDataService _persistentDataService;

        private int _coins;

        public int Coins
        {
            get => _coins;
            private set
            {
                if (value < 0)
                {
                    value = 0;
                }

                if (value != _coins)
                {
                    _coins = value;
                    OnCoinsChanged?.Invoke(this, new CoinsChangedEventArgs(_coins));
                }
            }
        }

        public event EventHandler<CoinsChangedEventArgs> OnCoinsChanged;

        public CoinService(IPersistentDataService persistentDataService)
        {
            _persistentDataService = persistentDataService;

            persistentDataService.OnResetProgress += OnResetProgress;
        }

        public void UpdateCoinsFromProgress()
        {
            _coins = _persistentDataService.Progress.Coins;
        }

        public void IncrementCoins()
        {
            Coins++;

            UpdateAndSaveProgress();
        }

        public void SpendCoins(int amount)
        {
            Coins -= amount;

            UpdateAndSaveProgress();
        }

        private void OnResetProgress(object sender, EventArgs e)
        {
            UpdateCoinsFromProgress();
        }

        private void UpdateAndSaveProgress()
        {
            _persistentDataService.Progress.Coins = _coins;
            _persistentDataService.Save();
        }
    }
}