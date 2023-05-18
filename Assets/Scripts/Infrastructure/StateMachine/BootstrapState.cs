using Controllers;

namespace Infrastructure.StateMachine
{
    public class BootstrapState : IState
    {
        private const string BootstrapSceneName = "BootstrapScene";
        private const string Level1SceneName = "Level_1";

        private readonly GameStateMachine _gameStateMachine;

        private readonly SceneLoader _sceneLoader;

        private readonly ParticleController _particleController;
        private readonly SoundController _soundController;
        private readonly ScreenFaderController _screenFaderController;
        private readonly SceneController _sceneController;

        private readonly GameData _gameData;

        public BootstrapState(GameStateMachine gameStateMachine, GameData gameData, SceneLoader sceneLoader,
            ParticleController particleController, SoundController soundController,
            ScreenFaderController screenFaderController, SceneController sceneController)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _particleController = particleController;
            _gameData = gameData;
            _soundController = soundController;
            _screenFaderController = screenFaderController;
            _sceneController = sceneController;
        }

        public void Enter()
        {
            RegisterServices();
            _sceneLoader.LoadScene(BootstrapSceneName, OnBootstrapSceneLoaded);
        }

        public void Exit()
        {
        }

        private void OnBootstrapSceneLoaded()
        {
            _gameStateMachine.Enter<LoadLevelState, string>(Level1SceneName);
        }

        private void RegisterServices()
        {
            AllServices.Instance.Register(_particleController, _soundController, _screenFaderController,
                _sceneController);
            AllServices.Instance.Init(_gameData);
        }
    }
}