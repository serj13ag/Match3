namespace Infrastructure.StateMachine
{
    public class BootstrapState : IState
    {
        private const string BootstrapSceneName = "BootstrapScene";
        private const string Level1SceneName = "Level_1";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(GameStateMachine gameStateMachine, GameData gameData, GlobalServices services)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = services.SceneLoader;

            services.InitGlobalServices(gameData);
        }

        public void Enter()
        {
            _sceneLoader.LoadScene(BootstrapSceneName, OnBootstrapSceneLoaded);
        }

        public void Exit()
        {
        }

        private void OnBootstrapSceneLoaded()
        {
            _gameStateMachine.Enter<LoadLevelState, string>(Level1SceneName);
        }
    }
}