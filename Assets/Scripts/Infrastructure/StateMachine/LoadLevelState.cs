using Controllers;
using Enums;
using Services;
using Services.PersistentProgress;
using UI;

namespace Infrastructure.StateMachine
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string ScoreControllerPath = "Prefabs/Infrastructure/Level/ScoreController";
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
        private readonly UpdateController _updateController;
        private readonly PersistentProgressService _persistentProgressService;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
            LoadingCurtainController loadingCurtainController, AssetProviderService assetProviderService,
            RandomService randomService, GameDataRepository gameDataRepository, SoundController soundController,
            UpdateController updateController, PersistentProgressService persistentProgressService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtainController = loadingCurtainController;
            _assetProviderService = assetProviderService;
            _randomService = randomService;
            _gameDataRepository = gameDataRepository;
            _soundController = soundController;
            _updateController = updateController;
            _persistentProgressService = persistentProgressService;
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

            BoardService boardService = new BoardService(particleController, gameFactory, _randomService, scoreController,
                _gameDataRepository, _soundController, _updateController, _persistentProgressService);

            UIController uiController = _assetProviderService.Instantiate<UIController>(UiControllerPath);
            uiController.Init(_loadingCurtainController);

            BackgroundUi backgroundUi = _assetProviderService.Instantiate<BackgroundUi>(BackgroundUiPath);
            backgroundUi.Init(cameraService);

            int scoreGoal = 3000;
            int movesLeft = 10;
            LevelStateService levelStateService = new LevelStateService(uiController, boardService, scoreController, _soundController, scoreGoal, movesLeft);

            cameraService.SetupCamera(BoardService.BoardSize);

            _soundController.PlaySound(SoundType.Music);
            uiController.ShowStartGameMessageWindow(scoreGoal, levelStateService.ChangeStateToPlaying);

            _gameStateMachine.Enter<GameLoopState>();
        }
    }
}