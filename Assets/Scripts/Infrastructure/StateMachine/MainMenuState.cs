using Constants;
using Services;
using Services.UI;

namespace Infrastructure.StateMachine
{
    public class MainMenuState : IState
    {
        private readonly SceneLoader _sceneLoader;
        private readonly IUiFactory _uiFactory;
        private readonly IStaticDataService _staticDataService;

        public MainMenuState(SceneLoader sceneLoader, IUiFactory uiFactory)
        {
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
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
            _uiFactory.CreateMainMenu();
        }
    }
}