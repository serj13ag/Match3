using Constants;
using Services;
using Services.Mono;
using Services.Mono.Sound;
using Services.UI;
using StaticData;
using UI.Background;

namespace Infrastructure.StateMachine
{
    public class GameLoopState : IPayloadedState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtainMonoService _loadingCurtainMonoService;
        private readonly AssetProviderService _assetProviderService;
        private readonly RandomService _randomService;
        private readonly StaticDataService _staticDataService;
        private readonly SoundMonoService _soundMonoService;
        private readonly UpdateMonoService _updateMonoService;
        private readonly PersistentProgressService _persistentProgressService;
        private readonly UiFactory _uiFactory;
        private readonly WindowService _windowService;
        private readonly SaveLoadService _saveLoadService;

        public GameLoopState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
            LoadingCurtainMonoService loadingCurtainMonoService, AssetProviderService assetProviderService,
            RandomService randomService, StaticDataService staticDataService, SoundMonoService soundMonoService,
            UpdateMonoService updateMonoService, PersistentProgressService persistentProgressService,
            UiFactory uiFactory, WindowService windowService, SaveLoadService saveLoadService)
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
            _uiFactory = uiFactory;
            _windowService = windowService;
            _saveLoadService = saveLoadService;
        }

        public void Enter(string levelName)
        {
            _loadingCurtainMonoService.FadeOnInstantly();
            _sceneLoader.LoadScene(Settings.GameLevelScene, () => OnLevelLoaded(levelName), true);
        }

        public void Exit()
        {
        }

        private void OnLevelLoaded(string levelName)
        {
            _uiFactory.CreateUiRootCanvas();

            LevelStaticData levelStaticData = _staticDataService.Levels[levelName];
            int scoreGoal = levelStaticData.ScoreGoal;
            int movesLeft = levelStaticData.MovesLeft;

            ParticleService particleService = new ParticleService(_staticDataService);
            GameFactory gameFactory = new GameFactory(_randomService, _staticDataService, particleService);
            GameRoundService gameRoundService = new GameRoundService(levelName, _gameStateMachine, _soundMonoService, _windowService);
            ScoreService scoreService = new ScoreService(gameRoundService, scoreGoal);

            BoardService boardService = new BoardService(levelName, _randomService, _staticDataService,
                _soundMonoService, _updateMonoService, _persistentProgressService, _saveLoadService, gameFactory,
                scoreService, gameRoundService, particleService);

            MovesLeftService movesLeftService = new MovesLeftService(boardService, scoreService, gameRoundService, movesLeft);
            CameraService cameraService = new CameraService(boardService.BoardSize);

            BackgroundScreen backgroundScreen = _assetProviderService.Instantiate<BackgroundScreen>(AssetPaths.BackgroundScreenPath);
            backgroundScreen.Init(scoreService, cameraService, movesLeftService);

            gameRoundService.StartGame(scoreGoal);
            _loadingCurtainMonoService.FadeOffWithDelay();
        }
    }
}