using Controllers;
using Infrastructure.StateMachine;

namespace Infrastructure
{
    public class Game
    {
        public GameStateMachine GameStateMachine { get; }

        public Game(GameData gameData, ICoroutineRunner coroutineRunner, LevelLoadingCurtain levelLoadingCurtain,
            ParticleController particleController, SoundController soundController,
            ScreenFaderController screenFaderController, SceneController sceneController)
        {
            SceneLoader sceneLoader = new SceneLoader(coroutineRunner);

            GameStateMachine = new GameStateMachine(gameData, sceneLoader, levelLoadingCurtain, particleController,
                soundController, screenFaderController, sceneController);
        }
    }
}