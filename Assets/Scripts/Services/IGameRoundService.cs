namespace Services
{
    public interface IGameRoundService
    {
        bool RoundIsActive { get; }
        void StartGame();
        void EndRound();
    }
}