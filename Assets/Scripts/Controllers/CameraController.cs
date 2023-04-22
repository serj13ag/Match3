using UnityEngine;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        private const float CameraPositionZ = -10f;

        [SerializeField] private Camera _camera;

        [SerializeField] private int _borderSize;

        public void SetupCamera(Vector2Int boardSize)
        {
            SetCameraPosition(boardSize);
            SetCameraAspectRatio(boardSize);
        }

        private void SetCameraPosition(Vector2Int boardSize)
        {
            float boardCenterX = (boardSize.x - 1) / 2f;
            float boardCenterY = (boardSize.y - 1) / 2f;

            _camera.transform.position = new Vector3(boardCenterX, boardCenterY, CameraPositionZ);
        }

        private void SetCameraAspectRatio(Vector2Int boardSize)
        {
            float screenAspectRatio = (float)Screen.width / Screen.height;

            float verticalSize = boardSize.y / 2f + _borderSize;
            float horizontalSize = (boardSize.x / 2f + _borderSize) / screenAspectRatio;

            _camera.orthographicSize = Mathf.Max(verticalSize, horizontalSize);
        }
    }
}