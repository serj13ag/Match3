using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class SceneLoader : ISceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void LoadScene(string name, Action onLoaded = null, bool forceReload = false)
        {
            if (!forceReload && SceneManager.GetActiveScene().name == name)
            {
                onLoaded?.Invoke();
                return;
            }

            _coroutineRunner.StartCoroutine(LoadSceneRoutine(name, onLoaded));
        }

        private IEnumerator LoadSceneRoutine(string name, Action onLoaded)
        {
            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(name);

            while (!loadSceneAsync.isDone)
            {
                yield return null;
            }

            onLoaded?.Invoke();
        }
    }
}