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
        private readonly GameDataService _gameDataService;
        private readonly SoundController _soundController;
        private readonly UpdateController _updateController;
        private readonly PersistentProgressService _persistentProgressService;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
            LoadingCurtainController loadingCurtainController, AssetProviderService assetProviderService,
            RandomService randomService, GameDataService gameDataService, SoundController soundController,
            UpdateController updateController, PersistentProgressService persistentProgressService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtainController = loadingCurtainController;
            _assetProviderService = assetProviderService;
            _randomService = randomService;
            _gameDataService = gameDataService;
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
            
            ParticleController particleController = _assetProviderService.Instantiate<ParticleController>(ParticleControllerPath);
            GameFactory gameFactory = new GameFactory(_randomService, _gameDataService, particleController);
            ScoreController scoreController = _assetProviderService.Instantiate<ScoreController>(ScoreControllerPath);

            int width = 7; //TODO to data
            int height = 9; //TODO to data
            BoardService boardService = new BoardService(particleController, gameFactory, _randomService, scoreController,
                _gameDataService, _soundController, _updateController, _persistentProgressService, width, height);

            UIController uiController = _assetProviderService.Instantiate<UIController>(UiControllerPath);
            uiController.Init(_loadingCurtainController);

            int scoreGoal = 3000; //TODO to data
            int movesLeft = 10; //TODO to data
            LevelStateService levelStateService = new LevelStateService(uiController, boardService, scoreController, _soundController, scoreGoal, movesLeft);

            CameraService cameraService = new CameraService(boardService.BoardSize);

            BackgroundUi backgroundUi = _assetProviderService.Instantiate<BackgroundUi>(BackgroundUiPath);
            backgroundUi.Init(cameraService);

            _soundController.PlaySound(SoundType.Music);
            uiController.ShowStartGameMessageWindow(scoreGoal, levelStateService.ChangeStateToPlaying);

            _gameStateMachine.Enter<GameLoopState>();
        }
    }
}