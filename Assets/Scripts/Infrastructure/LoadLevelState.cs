using Mono.Cecil;
using UnityEngine;

namespace Infrastructure
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly SceneLoader _sceneLoader;

        public LoadLevelState(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void Enter(string sceneName)
        {
            _sceneLoader.LoadScene(sceneName, OnLevelLoaded);
        }

        public void Exit()
        {
        }

        private void OnLevelLoaded()
        {
            GameObject core = Instantiate("Prefabs/Core");
        }

        private static GameObject Instantiate(string path)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }
    }
}