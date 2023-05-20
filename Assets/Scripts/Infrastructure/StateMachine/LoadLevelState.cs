using Controllers;

namespace Infrastructure.StateMachine
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string ScoreControllerPath = "Prefabs/ScoreController";
        private const string BoardPath = "Prefabs/Board";
        private const string UiControllerPath = "Prefabs/UIController";
        private const string GameStateControllerPath = "Prefabs/GameStateController";
        private const string BackgroundUiPath = "Prefabs/BackgroundUi";
        private const string ParticleControllerPath = "Prefabs/Infrastructure/Global/ParticleController";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LevelLoadingCurtain _levelLoadingCurtain;
        private readonly AssetProviderService _assetProviderService;
        private readonly RandomService _randomService;
        private readonly GameDataRepository _gameDataRepository;
        private readonly SoundController _soundController;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
            LevelLoadingCurtain levelLoadingCurtain, AssetProviderService assetProviderService,
            RandomService randomService, GameDataRepository gameDataRepository, SoundController soundController)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _levelLoadingCurtain = levelLoadingCurtain;
            _assetProviderService = assetProviderService;
            _randomService = randomService;
            _gameDataRepository = gameDataRepository;
            _soundController = soundController;
        }

        public void Enter(string sceneName)
        {
            _levelLoadingCurtain.FadeOnInstantly();
            _sceneLoader.LoadScene(sceneName, OnLevelLoaded);
        }

        public void Exit()
        {
            _levelLoadingCurtain.FadeOffWithDelay();
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
            uiController.Init(_levelLoadingCurtain);

            GameStateController gameStateController = _assetProviderService.Instantiate<GameStateController>(GameStateControllerPath);
            gameStateController.Init(uiController, board, cameraService, scoreController, _soundController);

            BackgroundUi backgroundUi = _assetProviderService.Instantiate<BackgroundUi>(BackgroundUiPath);
            backgroundUi.Init();

            gameStateController.InitializeLevel(10, 3000); // TODO move to levelData

            _gameStateMachine.Enter<GameLoopState>();
        }
    }
}