namespace Services
{
    public interface ICoinService : IService
    {
        int Coins { get; }
        void IncrementCoins();
    }
}