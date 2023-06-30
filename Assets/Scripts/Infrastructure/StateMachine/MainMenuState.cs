using Constants;
using Services;
using Services.UI;
using UI;

namespace Infrastructure.StateMachine
{
    public class MainMenuState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IUiFactory _uiFactory;
        private readonly IStaticDataService _staticDataService;

        public MainMenuState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
            IUiFactory uiFactory, IStaticDataService staticDataService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
            _staticDataService = staticDataService;
        }

        public void Enter()
        {
            _sceneLoader.LoadScene(Settings.MainMenuSceneName, OnSceneLoaded);
        }

        public void Exit()
        {
            _uiFactory.Cleanup();
        }

        private void OnSceneLoaded()
        {
            _uiFactory.CreateUiRootCanvas();
            
            MainMenu mainMenu = _uiFactory.GetMainMenu();
            mainMenu.Init(_gameStateMachine, _uiFactory, _staticDataService);
        }
    }
}