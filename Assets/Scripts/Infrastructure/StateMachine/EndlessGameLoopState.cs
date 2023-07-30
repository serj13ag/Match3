using Constants;
using Services;
using Services.Board;
using Services.GameRound;
using Services.Mono;
using Services.Mono.Sound;
using Services.MovesLeft;
using Services.UI;
using UI.Background;

namespace Infrastructure.StateMachine
{
    public class EndlessGameLoopState : IState
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

        public EndlessGameLoopState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
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

        public void Enter()
        {
            _loadingCurtainMonoService.FadeOnInstantly();
            _sceneLoader.LoadScene(Settings.GameLevelScene, OnSceneLoaded, true);
        }

        public void Exit()
        {
            Cleanup();
        }

        private void OnSceneLoaded()
        {
            _uiFactory.CreateUiRootCanvas();

            IProgressUpdateService progressUpdateService = new ProgressUpdateService(_persistentProgressService);

            IParticleService particleService = new ParticleService(_staticDataService);
            IGameFactory gameFactory = new GameFactory(_randomService, _staticDataService, particleService);
            IMovesLeftService movesLeftService = new InfiniteMovesLeftService();
            IPlayerLevelService playerLevelService = new PlayerLevelService(_persistentProgressService, _staticDataService, progressUpdateService);
            IScoreService scoreService = new ScoreService(Settings.EndlessLevelName, _soundMonoService, _persistentProgressService, progressUpdateService, playerLevelService.ScoreToNextLevel);
            IGameRoundService gameRoundService = new EndlessGameRoundService(_soundMonoService, _windowService, scoreService, playerLevelService);

            ITileService tileService = new TileService(Settings.EndlessLevelName, _staticDataService, progressUpdateService, gameFactory, gameRoundService);
            IGamePieceService gamePieceService = new GamePieceService(Settings.EndlessLevelName, _staticDataService, _soundMonoService,
                _randomService, progressUpdateService, tileService, gameFactory, particleService);

            IBoardService boardService = new BoardService(Settings.EndlessLevelName, _soundMonoService, _updateMonoService,
                _persistentProgressService, _staticDataService, progressUpdateService, scoreService, movesLeftService, gameRoundService,
                tileService, gamePieceService);

            ICameraService cameraService = new CameraService(boardService.BoardSize);

            EndlessBackgroundScreen endlessBackgroundScreen = _assetProviderService.Instantiate<EndlessBackgroundScreen>(AssetPaths.EndlessBackgroundScreenPath);
            endlessBackgroundScreen.Init(_gameStateMachine, playerLevelService, scoreService, cameraService);

            gameRoundService.StartGame();
            _loadingCurtainMonoService.FadeOffWithDelay();
        }

        private void Cleanup()
        {
            _uiFactory.Cleanup();
        }
    }
}