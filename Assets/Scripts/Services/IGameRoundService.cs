namespace Services
{
    public interface IGameRoundService
    {
        bool RoundIsActive { get; }
        void StartGame(int scoreGoal);
        void EndRound(bool scoreGoalReached);
    }
}