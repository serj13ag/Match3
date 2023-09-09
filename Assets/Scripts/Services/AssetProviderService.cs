using UnityEngine;

namespace Services
{
    public class AssetProviderService : IAssetProviderService
    {
        public T Instantiate<T>(string path) where T : Object
        {
            T prefab = LoadAsset<T>(path);
            T instance = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            instance.name = prefab.name;

            return instance;
        }

        public T Instantiate<T>(string path, Transform parentTransform) where T : Object
        {
            T prefab = LoadAsset<T>(path);
            T instance = Object.Instantiate(prefab, parentTransform);
            instance.name = prefab.name;

            return instance;
        }

        public Sprite LoadSprite(string path)
        {
            return LoadAsset<Sprite>(path);
        }

        private static T LoadAsset<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }
    }
}