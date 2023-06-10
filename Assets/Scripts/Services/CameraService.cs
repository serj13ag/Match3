using Constants;
using UnityEngine;

namespace Services
{
    public class CameraService : ICameraService
    {
        private const int BorderSize = 2;

        public Camera MainCamera { get; }

        public CameraService(Vector2Int boardSize)
        {
            MainCamera = Camera.main;

            SetCameraPosition(boardSize);
            SetCameraAspectRatio(boardSize);
        }

        private void SetCameraPosition(Vector2Int boardSize)
        {
            float boardCenterX = (boardSize.x - 1) / 2f;
            float boardCenterY = (boardSize.y - 1) / 2f;

            MainCamera.transform.position = new Vector3(boardCenterX, boardCenterY, Settings.CameraPositionZ);
        }

        private void SetCameraAspectRatio(Vector2Int boardSize)
        {
            float screenAspectRatio = (float)Screen.width / Screen.height;

            float verticalSize = boardSize.y / 2f + BorderSize;
            float horizontalSize = (boardSize.x / 2f + BorderSize) / screenAspectRatio;

            MainCamera.orthographicSize = Mathf.Max(verticalSize, horizontalSize);
        }
    }
}