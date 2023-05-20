using UnityEngine;

namespace Infrastructure
{
    public class AssetProviderService
    {
        public T Instantiate<T>(string path) where T : Object
        {
            T prefab = Resources.Load<T>(path);
            return Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }
    }
}