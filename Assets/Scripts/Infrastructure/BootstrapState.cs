namespace Infrastructure
{
    public class BootstrapState : IState
    {
        private const string BootstrapSceneName = "BootstrapScene";
        private const string Level1SceneName = "Level_1";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
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
        }
    }
}