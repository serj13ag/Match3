namespace Infrastructure.StateMachine
{
    public class BootstrapState : IState
    {
        private const string BootstrapSceneName = "BootstrapScene";
        private const string Level1SceneName = "Level_1";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly GameData _gameData;

        public BootstrapState(GameStateMachine gameStateMachine, GameData gameData, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _gameData = gameData;
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
            AllServices.Instance.InitGlobalServices(_gameData);

            _gameStateMachine.Enter<LoadLevelState, string>(Level1SceneName);
        }
    }
}