using Infrastructure.StateMachine;

namespace Infrastructure
{
    public class Game
    {
        public GameStateMachine GameStateMachine { get; }

        public Game(GameData gameData, ICoroutineRunner coroutineRunner)
        {
            SceneLoader sceneLoader = new SceneLoader(coroutineRunner);
            GlobalServices globalServices = new GlobalServices(sceneLoader);

            GameStateMachine = new GameStateMachine(gameData, globalServices);
        }
    }
}