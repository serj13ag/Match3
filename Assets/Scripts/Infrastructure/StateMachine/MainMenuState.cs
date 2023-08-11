using Constants;
using Services.Mono;
using Services.Mono.Sound;
using Services.UI;
using UI;

namespace Infrastructure.StateMachine
{
    public class MainMenuState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly IUiFactory _uiFactory;
        private readonly ISoundMonoService _soundMonoService;
        private readonly ILoadingCurtainMonoService _loadingCurtainMonoService;
        private readonly IWindowService _windowService;

        public bool IsGameLoopState => false;

        public MainMenuState(IGameStateMachine gameStateMachine, ISceneLoader sceneLoader, IUiFactory uiFactory,
            ISoundMonoService soundMonoService, ILoadingCurtainMonoService loadingCurtainMonoService,
            IWindowService windowService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
            _soundMonoService = soundMonoService;
            _loadingCurtainMonoService = loadingCurtainMonoService;
            _windowService = windowService;
        }

        public void Enter()
        {
            _loadingCurtainMonoService.FadeOnInstantly();
            _sceneLoader.LoadScene(Settings.MainMenuSceneName, OnSceneLoaded);
        }

        public void Exit()
        {
            _uiFactory.Cleanup();
            _windowService.Cleanup();
        }

        private void OnSceneLoaded()
        {
            _uiFactory.CreateUiRootCanvas();

            MainMenu mainMenu = _uiFactory.CreateMainMenu();
            mainMenu.Init(_gameStateMachine, _windowService);

            _soundMonoService.PlayBackgroundMusic();

            _loadingCurtainMonoService.FadeOffWithDelay();
        }
    }
}