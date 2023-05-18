using Controllers;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string ScoreControllerPath = "Prefabs/ScoreController";
        private const string BoardPath = "Prefabs/Board";
        private const string UiControllerPath = "Prefabs/UIController";
        private const string GameStateControllerPath = "Prefabs/GameStateController";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LevelLoadingCurtain _levelLoadingCurtain;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader,
            LevelLoadingCurtain levelLoadingCurtain)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _levelLoadingCurtain = levelLoadingCurtain;
        }

        public void Enter(string sceneName)
        {
            _levelLoadingCurtain.FadeOnInstantly();
            _sceneLoader.LoadScene(sceneName, OnLevelLoaded);
        }

        public void Exit()
        {
            _levelLoadingCurtain.FadeOffWithDelay();
        }

        private void OnLevelLoaded()
        {
            ScoreController scoreController = Instantiate<ScoreController>(ScoreControllerPath);
            Board board = Instantiate<Board>(BoardPath);
            UIController uiController = Instantiate<UIController>(UiControllerPath);
            GameStateController gameStateController = Instantiate<GameStateController>(GameStateControllerPath);

            board.Init(AllServices.Instance.ParticleController, AllServices.Instance.Factory,
                AllServices.Instance.RandomService, scoreController, AllServices.Instance.GameDataRepository,
                AllServices.Instance.SoundController);

            uiController.Init(AllServices.Instance.ScreenFaderController);
            gameStateController.Init(uiController, board, AllServices.Instance.CameraService, scoreController,
                AllServices.Instance.SoundController);

            gameStateController.InitializeLevel(10, 3000); // TODO move to levelData

            _gameStateMachine.Enter<GameLoopState>();
        }

        private static T Instantiate<T>(string path) where T : Object
        {
            T prefab = Resources.Load<T>(path);
            return Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }
    }
}