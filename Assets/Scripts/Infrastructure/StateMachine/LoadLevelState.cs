using Enums;
using Services;
using Services.Mono;
using Services.PersistentProgress;
using StaticData;
using UI;

namespace Infrastructure.StateMachine
{
    public class LoadLevelState : IPayloadedState<string>
    {
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

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
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

        public void Enter(string sceneName)
        {
            _loadingCurtainMonoService.FadeOnInstantly();
            _sceneLoader.LoadScene(sceneName, OnLevelLoaded);
        }

        public void Exit()
        {
            _loadingCurtainMonoService.FadeOffWithDelay();
        }

        private void OnLevelLoaded()
        {
            ParticleService particleService = new ParticleService(_staticDataService);
            GameFactory gameFactory = new GameFactory(_randomService, _staticDataService, particleService);
            ScoreService scoreService = new ScoreService();

            string levelName = Constants.FirstLevelName; // TODO add to progress
            LevelStaticData levelStaticData = _staticDataService.Levels[levelName];
            int scoreGoal = levelStaticData.ScoreGoal;
            int movesLeft = levelStaticData.MovesLeft;

            BoardService boardService = new BoardService(levelName, particleService, gameFactory, _randomService,
                scoreService, _staticDataService, _soundMonoService, _updateMonoService, _persistentProgressService);

            UiMonoService uiMonoService = _assetProviderService.Instantiate<UiMonoService>(UiMonoServicePath);
            uiMonoService.Init(_loadingCurtainMonoService);

            LevelStateService levelStateService = new LevelStateService(uiMonoService, boardService, scoreService,
                _soundMonoService, scoreGoal, movesLeft);
            CameraService cameraService = new CameraService(boardService.BoardSize);

            BackgroundUi backgroundUi = _assetProviderService.Instantiate<BackgroundUi>(BackgroundUiPath);
            backgroundUi.Init(scoreService, cameraService);

            _soundMonoService.PlaySound(SoundType.Music);
            uiMonoService.ShowStartGameMessageWindow(scoreGoal, levelStateService.ChangeStateToPlaying);

            _gameStateMachine.Enter<GameLoopState>();
        }
    }
}