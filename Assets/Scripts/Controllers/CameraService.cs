using UnityEngine;

namespace Controllers
{
    public class CameraService
    {
        private const int BorderSize = 2;

        public void SetupCamera(Vector2Int boardSize)
        {
            SetCameraPosition(boardSize);
            SetCameraAspectRatio(boardSize);
        }

        private void SetCameraPosition(Vector2Int boardSize)
        {
            float boardCenterX = (boardSize.x - 1) / 2f;
            float boardCenterY = (boardSize.y - 1) / 2f;

            Camera.main.transform.position = new Vector3(boardCenterX, boardCenterY, Constants.CameraPositionZ);
        }

        private void SetCameraAspectRatio(Vector2Int boardSize)
        {
            float screenAspectRatio = (float)Screen.width / Screen.height;

            float verticalSize = boardSize.y / 2f + BorderSize;
            float horizontalSize = (boardSize.x / 2f + BorderSize) / screenAspectRatio;

            Camera.main.orthographicSize = Mathf.Max(verticalSize, horizontalSize);
        }
    }
}