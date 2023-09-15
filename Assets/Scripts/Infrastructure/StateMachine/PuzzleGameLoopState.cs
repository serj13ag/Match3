using Constants;
using Services;
using Services.Board;
using Services.GameRound;
using Services.Mono;
using Services.Mono.Sound;
using Services.MovesLeft;
using Services.UI;
using StaticData;
using UI.Background;

namespace Infrastructure.StateMachine
{
    public class PuzzleGameLoopState : IPayloadedState<string>
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly ILoadingCurtainMonoService _loadingCurtainMonoService;
        private readonly IAssetProviderService _assetProviderService;
        private readonly IRandomService _randomService;
        private readonly IStaticDataService _staticDataService;
        private readonly ISoundMonoService _soundMonoService;
        private readonly IUpdateMonoService _updateMonoService;
        private readonly IPersistentDataService _persistentDataService;
        private readonly IUiFactory _uiFactory;
        private readonly IWindowService _windowService;

        public bool IsGameLoopState => true;

        public PuzzleGameLoopState(IGameStateMachine gameStateMachine, ISceneLoader sceneLoader,
            ILoadingCurtainMonoService loadingCurtainMonoService, IAssetProviderService assetProviderService,
            IRandomService randomService, IStaticDataService staticDataService, ISoundMonoService soundMonoService,
            IUpdateMonoService updateMonoService, IPersistentDataService persistentDataService,
            IUiFactory uiFactory, IWindowService windowService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtainMonoService = loadingCurtainMonoService;
            _assetProviderService = assetProviderService;
            _randomService = randomService;
            _staticDataService = staticDataService;
            _soundMonoService = soundMonoService;
            _updateMonoService = updateMonoService;
            _persistentDataService = persistentDataService;
            _uiFactory = uiFactory;
            _windowService = windowService;
        }

        public void Enter(string levelName)
        {
            _loadingCurtainMonoService.FadeOnInstantly();
            _sceneLoader.LoadScene(Settings.GameLevelScene, () => OnSceneLoaded(levelName), true);
        }

        public void Exit()
        {
            Cleanup();
        }

        private void OnSceneLoaded(string levelName)
        {
            _uiFactory.CreateUiRootCanvas();

            LevelStaticData levelStaticData = _staticDataService.GetDataForLevel(levelName);
            int scoreGoal = levelStaticData.ScoreGoal;
            int movesLeft = levelStaticData.MovesLeft;

            IProgressUpdateService progressUpdateService = new ProgressUpdateService(levelName, _persistentDataService);

            IParticleService particleService = new ParticleService(_staticDataService);
            IGameFactory gameFactory = new GameFactory(levelName, _randomService, _staticDataService, particleService);
            IMovesLeftService movesLeftService = new MovesLeftService(levelName, _persistentDataService, progressUpdateService, movesLeft);
            IScoreService scoreService = new ScoreService(levelName, _soundMonoService, _persistentDataService, progressUpdateService, scoreGoal);
            IGameRoundService gameRoundService = new PuzzleGameRoundService(levelName, _gameStateMachine, _soundMonoService, _windowService, _persistentDataService, scoreService);

            ITileService tileService = new TileService(levelName, _staticDataService, progressUpdateService, gameFactory, gameRoundService);
            IGamePieceService gamePieceService = new GamePieceService(levelName, _staticDataService, _soundMonoService,
                _randomService, progressUpdateService, tileService, gameFactory, particleService);

            IBoardService boardService = new BoardService(levelName, _soundMonoService, _updateMonoService,
                _persistentDataService, _staticDataService, progressUpdateService, scoreService, movesLeftService, gameRoundService,
                tileService, gamePieceService, particleService);

            ICameraService cameraService = new CameraService(boardService.BoardSize);

            PuzzleBackgroundScreen puzzleBackgroundScreen = _assetProviderService.Instantiate<PuzzleBackgroundScreen>(AssetPaths.PuzzleBackgroundScreenPath);
            puzzleBackgroundScreen.Init(levelName, scoreService, cameraService, movesLeftService, _windowService);

            gameRoundService.StartGame();
            _loadingCurtainMonoService.FadeOffWithDelay();
        }

        private void Cleanup()
        {
            _uiFactory.Cleanup();
        }
    }
}