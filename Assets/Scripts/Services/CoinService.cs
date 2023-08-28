namespace Services
{
    public class CoinService : ICoinService
    {
        private readonly IPersistentProgressService _persistentProgressService;

        private int _coins;

        public int Coins => _coins;

        public CoinService(IPersistentProgressService persistentProgressService)
        {
            _persistentProgressService = persistentProgressService;

            _coins = persistentProgressService.Progress.Coins;
        }

        public void IncrementCoins()
        {
            _coins++;

            _persistentProgressService.Progress.Coins = _coins;
            _persistentProgressService.SaveProgress();
        }
    }
}