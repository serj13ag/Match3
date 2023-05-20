using Infrastructure.StateMachine;

namespace Infrastructure
{
    public class Game
    {
        public GameStateMachine GameStateMachine { get; }

        public Game(GameData gameData, ICoroutineRunner coroutineRunner)
        {
            SceneLoader sceneLoader = new SceneLoader(coroutineRunner);

            GameStateMachine = new GameStateMachine(gameData, sceneLoader);
        }
    }
}