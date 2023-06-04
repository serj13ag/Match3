using UnityEngine;

namespace Services
{
    public class AssetProviderService
    {
        public T Instantiate<T>(string path) where T : Object
        {
            T prefab = LoadAsset<T>(path);
            return Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }

        public T Instantiate<T>(string path, Transform parentTransform) where T : Object
        {
            T prefab = LoadAsset<T>(path);
            return Object.Instantiate(prefab, parentTransform);
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