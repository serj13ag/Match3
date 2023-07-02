using Constants;
using Services;
using Services.Mono;
using Services.Mono.Sound;
using Services.UI;

namespace Infrastructure.StateMachine
{
    public class MainMenuState : IState
    {
        private readonly SceneLoader _sceneLoader;
        private readonly IUiFactory _uiFactory;
        private readonly ISoundMonoService _soundMonoService;
        private readonly ILoadingCurtainMonoService _loadingCurtainMonoService;
        private readonly IStaticDataService _staticDataService;

        public MainMenuState(SceneLoader sceneLoader, IUiFactory uiFactory,
            ISoundMonoService soundMonoService, ILoadingCurtainMonoService loadingCurtainMonoService)
        {
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;
            _soundMonoService = soundMonoService;
            _loadingCurtainMonoService = loadingCurtainMonoService;
        }

        public void Enter()
        {
            _loadingCurtainMonoService.FadeOnInstantly();
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

            _soundMonoService.PlayBackgroundMusic();

            _loadingCurtainMonoService.FadeOffWithDelay();
        }
    }
}