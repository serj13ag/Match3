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
        private readonly ILoadingCurtainMonoService _loadingCurtainMonoService;
        private readonly IAssetProviderService _assetProviderService;
        private readonly IRandomService _randomService;
        private readonly IStaticDataService _staticDataService;
        private readonly ISoundMonoService _soundMonoService;
        private readonly IUpdateMonoService _updateMonoService;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly IUiFactory _uiFactory;
        private readonly IWindowService _windowService;
        private readonly ISaveLoadService _saveLoadService;

        public GameLoopState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
            ILoadingCurtainMonoService loadingCurtainMonoService, IAssetProviderService assetProviderService,
            IRandomService randomService, IStaticDataService staticDataService, ISoundMonoService soundMonoService,
            IUpdateMonoService updateMonoService, IPersistentProgressService persistentProgressService,
            IUiFactory uiFactory, IWindowService windowService, ISaveLoadService saveLoadService)
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

            LevelStaticData levelStaticData = _staticDataService.GetDataForLevel(levelName);
            int scoreGoal = levelStaticData.ScoreGoal;
            int movesLeft = levelStaticData.MovesLeft;

            IParticleService particleService = new ParticleService(_staticDataService);
            IGameFactory gameFactory = new GameFactory(_randomService, _staticDataService, particleService);
            IGameRoundService gameRoundService = new GameRoundService(levelName, _gameStateMachine, _soundMonoService, _windowService);
            IScoreService scoreService = new ScoreService(gameRoundService, scoreGoal);

            ITileService tileService = new TileService(levelName, _staticDataService, _persistentProgressService, gameFactory);

            IBoardService boardService = new BoardService(levelName, _randomService, _staticDataService,
                _soundMonoService, _updateMonoService, _persistentProgressService, _saveLoadService, gameFactory,
                scoreService, gameRoundService, particleService, tileService);

            IMovesLeftService movesLeftService = new MovesLeftService(boardService, scoreService, gameRoundService, movesLeft);
            ICameraService cameraService = new CameraService(boardService.BoardSize);

            BackgroundScreen backgroundScreen = _assetProviderService.Instantiate<BackgroundScreen>(AssetPaths.BackgroundScreenPath);
            backgroundScreen.Init(scoreService, cameraService, movesLeftService);

            gameRoundService.StartGame(scoreGoal);
            _loadingCurtainMonoService.FadeOffWithDelay();
        }
    }
}