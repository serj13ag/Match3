namespace Infrastructure
{
    public class Game
    {
        private readonly GameStateMachine _gameStateMachine;

        public GameStateMachine GameStateMachine => _gameStateMachine;

        public Game(ICoroutineRunner coroutineRunner)
        {
            SceneLoader sceneLoader = new SceneLoader(coroutineRunner);
            _gameStateMachine = new GameStateMachine(sceneLoader);
        }
    }
}