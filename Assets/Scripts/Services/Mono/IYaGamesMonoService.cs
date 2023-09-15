using System;

namespace Services.Mono
{
    public interface IYaGamesMonoService : IService
    {
        void Save(string key, string jsonDataString);
        void Load(string key, Action<string> onLoadedCallback);
    }
}