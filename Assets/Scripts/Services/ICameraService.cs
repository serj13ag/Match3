using UnityEngine;

namespace Services
{
    public interface ICameraService
    {
        Camera MainCamera { get; }
        void UpdateAspectRatio();
    }
}