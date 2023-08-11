using System;
using Services;

namespace Infrastructure
{
    public interface ISceneLoader : IService
    {
        void LoadScene(string name, Action onLoaded = null, bool forceReload = false);
    }
}