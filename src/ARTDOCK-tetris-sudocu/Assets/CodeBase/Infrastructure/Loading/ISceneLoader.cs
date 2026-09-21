using System;

namespace CodeBase.Infrastructure.Loading
{
    public interface ISceneLoader
    {
        void LoadScene(string name, Action onLoaded = null);
        void RestartScene(string name, Action onLoaded = null);
    }
}