using Enums;
using Services;
using Services.Mono;
using Services.PersistentProgress;
using UI;

namespace Infrastructure.StateMachine
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string ScoreMonoServicePath = "Prefabs/Services/Level/ScoreMonoService";
        private const string UiMonoServicePath = "Prefabs/Services/Level/UiMonoService";
        private const string BackgroundUiPath = "Prefabs/Services/Level/BackgroundUi";
        private const string ParticleMonoServicePath = "Prefabs/Services/Level/ParticleMonoService";

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
            
            ParticleMonoService particleMonoService = _assetProviderService.Instantiate<ParticleMonoService>(ParticleMonoServicePath);
            GameFactory gameFactory = new GameFactory(_randomService, _staticDataService, particleMonoService);
            ScoreMonoService scoreMonoService = _assetProviderService.Instantiate<ScoreMonoService>(ScoreMonoServicePath);

            int width = 7; //TODO to data
            int height = 9; //TODO to data
            BoardService boardService = new BoardService(particleMonoService, gameFactory, _randomService, scoreMonoService,
                _staticDataService, _soundMonoService, _updateMonoService, _persistentProgressService, width, height);

            UiMonoService uiMonoService = _assetProviderService.Instantiate<UiMonoService>(UiMonoServicePath);
            uiMonoService.Init(_loadingCurtainMonoService);

            int scoreGoal = 3000; //TODO to data
            int movesLeft = 10; //TODO to data
            LevelStateService levelStateService = new LevelStateService(uiMonoService, boardService, scoreMonoService, _soundMonoService, scoreGoal, movesLeft);

            CameraService cameraService = new CameraService(boardService.BoardSize);

            BackgroundUi backgroundUi = _assetProviderService.Instantiate<BackgroundUi>(BackgroundUiPath);
            backgroundUi.Init(cameraService);

            _soundMonoService.PlaySound(SoundType.Music);
            uiMonoService.ShowStartGameMessageWindow(scoreGoal, levelStateService.ChangeStateToPlaying);

            _gameStateMachine.Enter<GameLoopState>();
        }
    }
}