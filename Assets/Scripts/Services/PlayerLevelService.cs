namespace Services
{
    public class PlayerLevelService : IPlayerLevelService
    {
        private const int ScoreToNextLevelConst = 500;

        private int _scoreToNextLevel;

        public int ScoreToNextLevel => _scoreToNextLevel;

        public PlayerLevelService()
        {
            _scoreToNextLevel = ScoreToNextLevelConst;
        }

        public void GoToNextLevel()
        {
            _scoreToNextLevel = ScoreToNextLevelConst;
        }
    }
}