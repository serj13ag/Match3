namespace Services
{
    public interface IPlayerLevelService
    {
        int ScoreToNextLevel { get; }
        void GoToNextLevel();
    }
}