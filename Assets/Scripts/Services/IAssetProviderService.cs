using UnityEngine;

namespace Services
{
    public interface IAssetProviderService : IService
    {
        T Instantiate<T>(string path) where T : Object;
        T Instantiate<T>(string path, Transform parentTransform) where T : Object;
        Sprite LoadSprite(string path);
    }
}