using UnityEngine;

namespace Infrastructure
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string CorePrefabPath = "Prefabs/Core";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LevelLoadingCurtain _levelLoadingCurtain;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LevelLoadingCurtain levelLoadingCurtain)
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
            GameObject core = Instantiate(CorePrefabPath);

            _gameStateMachine.Enter<GameLoopState>();
        }

        private static GameObject Instantiate(string path)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }
    }
}