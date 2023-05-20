using Controllers;

namespace Infrastructure.StateMachine
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string ScoreControllerPath = "Prefabs/Infrastructure/Level/ScoreController";
        private const string BoardPath = "Prefabs/Infrastructure/Level/Board";
        private const string UiControllerPath = "Prefabs/Infrastructure/Level/UIController";
        private const string BackgroundUiPath = "Prefabs/Infrastructure/Level/BackgroundUi";
        private const string ParticleControllerPath = "Prefabs/Infrastructure/Level/ParticleController";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtainController _loadingCurtainController;
        private readonly AssetProviderService _assetProviderService;
        private readonly RandomService _randomService;
        private readonly GameDataRepository _gameDataRepository;
        private readonly SoundController _soundController;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
            LoadingCurtainController loadingCurtainController, AssetProviderService assetProviderService,
            RandomService randomService, GameDataRepository gameDataRepository, SoundController soundController)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtainController = loadingCurtainController;
            _assetProviderService = assetProviderService;
            _randomService = randomService;
            _gameDataRepository = gameDataRepository;
            _soundController = soundController;
        }

        public void Enter(string sceneName)
        {
            _loadingCurtainController.FadeOnInstantly();
            _sceneLoader.LoadScene(sceneName, OnLevelLoaded);
        }

        public void Exit()
        {
            _loadingCurtainController.FadeOffWithDelay();
        }

        private void OnLevelLoaded()
        {
            CameraService cameraService = new CameraService();
            ParticleController particleController = _assetProviderService.Instantiate<ParticleController>(ParticleControllerPath);
            GameFactory gameFactory = new GameFactory(_randomService, _gameDataRepository, particleController);
            ScoreController scoreController = _assetProviderService.Instantiate<ScoreController>(ScoreControllerPath);

            Board board = _assetProviderService.Instantiate<Board>(BoardPath);
            board.Init(particleController, gameFactory, _randomService, scoreController, _gameDataRepository,
                _soundController);

            UIController uiController = _assetProviderService.Instantiate<UIController>(UiControllerPath);
            uiController.Init(_loadingCurtainController);

            BackgroundUi backgroundUi = _assetProviderService.Instantiate<BackgroundUi>(BackgroundUiPath);
            backgroundUi.Init(cameraService);

            LevelStateService levelStateService = new LevelStateService(uiController, board, cameraService, scoreController, _soundController);

            levelStateService.InitializeLevel(10, 3000); // TODO move to levelData

            _gameStateMachine.Enter<GameLoopState>();
        }
    }
}