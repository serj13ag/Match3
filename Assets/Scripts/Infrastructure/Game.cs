namespace Infrastructure
{
    public class Game
    {
        private readonly GameStateMachine _gameStateMachine;

        public GameStateMachine GameStateMachine => _gameStateMachine;

        public Game(ICoroutineRunner coroutineRunner, LevelLoadingCurtain levelLoadingCurtain)
        {
            SceneLoader sceneLoader = new SceneLoader(coroutineRunner);
            _gameStateMachine = new GameStateMachine(sceneLoader, levelLoadingCurtain);
        }
    }
}