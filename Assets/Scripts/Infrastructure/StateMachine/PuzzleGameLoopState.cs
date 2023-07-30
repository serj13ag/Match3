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

        public PuzzleGameLoopState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
            ILoadingCurtainMonoService loadingCurtainMonoService, IAssetProviderService assetProviderService,
            IRandomService randomService, IStaticDataService staticDataService, ISoundMonoService soundMonoService,
            IUpdateMonoService updateMonoService, IPersistentProgressService persistentProgressService,
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
            _persistentProgressService = persistentProgressService;
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

            IProgressUpdateService progressUpdateService = new ProgressUpdateService(_persistentProgressService);

            IParticleService particleService = new ParticleService(_staticDataService);
            IGameFactory gameFactory = new GameFactory(_randomService, _staticDataService, particleService);
            IMovesLeftService movesLeftService = new MovesLeftService(levelName, _persistentProgressService, progressUpdateService, movesLeft);
            IScoreService scoreService = new ScoreService(levelName, _soundMonoService, _persistentProgressService, progressUpdateService, scoreGoal);
            IGameRoundService gameRoundService = new GameRoundService(levelName, _gameStateMachine, _soundMonoService, _windowService, _persistentProgressService, scoreService);

            ITileService tileService = new TileService(levelName, _staticDataService, progressUpdateService, gameFactory, gameRoundService);
            IGamePieceService gamePieceService = new GamePieceService(levelName, _staticDataService, _soundMonoService,
                _randomService, progressUpdateService, tileService, gameFactory, particleService);

            IBoardService boardService = new BoardService(levelName, _soundMonoService, _updateMonoService,
                _persistentProgressService, _staticDataService, progressUpdateService, scoreService, movesLeftService, gameRoundService,
                tileService, gamePieceService);

            ICameraService cameraService = new CameraService(boardService.BoardSize);

            PuzzleBackgroundScreen puzzleBackgroundScreen = _assetProviderService.Instantiate<PuzzleBackgroundScreen>(AssetPaths.PuzzleBackgroundScreenPath);
            puzzleBackgroundScreen.Init(levelName, _gameStateMachine, scoreService, cameraService, movesLeftService);

            gameRoundService.StartGame();
            _loadingCurtainMonoService.FadeOffWithDelay();
        }

        private void Cleanup()
        {
            _uiFactory.Cleanup();
        }
    }
}