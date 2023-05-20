using Controllers;

namespace Infrastructure.StateMachine
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string ScoreControllerPath = "Prefabs/ScoreController";
        private const string BoardPath = "Prefabs/Board";
        private const string UiControllerPath = "Prefabs/UIController";
        private const string GameStateControllerPath = "Prefabs/GameStateController";
        private const string BackgroundUiPath = "Prefabs/BackgroundUi";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter(string sceneName)
        {
            AllServices.Instance.LevelLoadingCurtain.FadeOnInstantly();
            _sceneLoader.LoadScene(sceneName, OnLevelLoaded);
        }

        public void Exit()
        {
            AllServices.Instance.LevelLoadingCurtain.FadeOffWithDelay();
        }

        private void OnLevelLoaded()
        {
            ScoreController scoreController = AllServices.Instance.AssetProviderService.Instantiate<ScoreController>(ScoreControllerPath);
            Board board = AllServices.Instance.AssetProviderService.Instantiate<Board>(BoardPath);
            UIController uiController = AllServices.Instance.AssetProviderService.Instantiate<UIController>(UiControllerPath);
            GameStateController gameStateController = AllServices.Instance.AssetProviderService.Instantiate<GameStateController>(GameStateControllerPath);
            BackgroundUi backgroundUi = AllServices.Instance.AssetProviderService.Instantiate<BackgroundUi>(BackgroundUiPath);

            board.Init(AllServices.Instance.ParticleController, AllServices.Instance.Factory,
                AllServices.Instance.RandomService, scoreController, AllServices.Instance.GameDataRepository,
                AllServices.Instance.SoundController);

            uiController.Init(AllServices.Instance.LevelLoadingCurtain);
            gameStateController.Init(uiController, board, AllServices.Instance.CameraService, scoreController,
                AllServices.Instance.SoundController);

            gameStateController.InitializeLevel(10, 3000); // TODO move to levelData
            
            backgroundUi.Init();

            _gameStateMachine.Enter<GameLoopState>();
        }
    }
}