using Services;
using Services.Mono;
using Services.PersistentProgress;
using StaticData;
using UI;

namespace Infrastructure.StateMachine
{
    public class GameLoopState : IPayloadedState<string>
    {
        private const string GameLevelScene = "GameLevelScene";

        private const string UiMonoServicePath = "Prefabs/Services/Level/UiMonoService";
        private const string BackgroundUiPath = "Prefabs/Services/Level/BackgroundUi";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtainMonoService _loadingCurtainMonoService;
        private readonly AssetProviderService _assetProviderService;
        private readonly RandomService _randomService;
        private readonly StaticDataService _staticDataService;
        private readonly SoundMonoService _soundMonoService;
        private readonly UpdateMonoService _updateMonoService;
        private readonly PersistentProgressService _persistentProgressService;

        public GameLoopState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
            LoadingCurtainMonoService loadingCurtainMonoService, AssetProviderService assetProviderService,
            RandomService randomService, StaticDataService staticDataService, SoundMonoService soundMonoService,
            UpdateMonoService updateMonoService, PersistentProgressService persistentProgressService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtainMonoService = loadingCurtainMonoService;
            _assetProviderService = assetProviderService;
            _randomService = randomService;
            _staticDataService = staticDataService;
            _soundMonoService = soundMonoService;
            _updateMonoService = updateMonoService;
            _persistentProgressService = persistentProgressService;
        }

        public void Enter(string levelName)
        {
            _loadingCurtainMonoService.FadeOnInstantly();
            _sceneLoader.LoadScene(GameLevelScene, () => OnLevelLoaded(levelName), true);
        }

        public void Exit()
        {
        }

        private void OnLevelLoaded(string levelName)
        {
            LevelStaticData levelStaticData = _staticDataService.Levels[levelName];
            int scoreGoal = levelStaticData.ScoreGoal;
            int movesLeft = levelStaticData.MovesLeft;

            UiMonoService uiMonoService = _assetProviderService.Instantiate<UiMonoService>(UiMonoServicePath);

            ParticleService particleService = new ParticleService(_staticDataService);
            GameFactory gameFactory = new GameFactory(_randomService, _staticDataService, particleService);
            GameRoundService gameRoundService = new GameRoundService(levelName, _gameStateMachine, _soundMonoService, uiMonoService);
            ScoreService scoreService = new ScoreService(gameRoundService, scoreGoal);

            BoardService boardService = new BoardService(levelName, _randomService, _staticDataService,
                _soundMonoService, _updateMonoService, _persistentProgressService, gameFactory, scoreService,
                gameRoundService, particleService);

            MovesLeftService movesLeftService = new MovesLeftService(boardService, scoreService, gameRoundService, movesLeft);
            CameraService cameraService = new CameraService(boardService.BoardSize);

            BackgroundUi backgroundUi = _assetProviderService.Instantiate<BackgroundUi>(BackgroundUiPath);
            backgroundUi.Init(scoreService, cameraService, movesLeftService);

            gameRoundService.StartGame(scoreGoal);
            _loadingCurtainMonoService.FadeOffWithDelay();
        }
    }
}