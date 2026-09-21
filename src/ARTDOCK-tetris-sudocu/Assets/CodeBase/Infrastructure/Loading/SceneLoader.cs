using System;
using System.Collections;
using CodeBase.Infrastructure.CoroutineRunner;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.Loading
{
    public class SceneLoader : ISceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void LoadScene(string name, Action onLoaded = null)
        {
            if (SceneManager.GetActiveScene().name == name)
            {
                onLoaded?.Invoke();
                return;
            }
            
            _coroutineRunner.RunCoroutine(Load(name, onLoaded));
        }
        
        public void RestartScene(string name, Action onLoaded = null)
        {
            _coroutineRunner.RunCoroutine(Load(name, onLoaded));
        }

        private IEnumerator Load(string nextScene, Action onLoaded)
        {
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);

            while (!waitNextScene.isDone)
                yield return null;

            onLoaded?.Invoke();
        }
    }
}