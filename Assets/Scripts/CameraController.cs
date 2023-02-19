using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float CameraPositionZ = -10f;

    [SerializeField] private Camera _camera;

    [SerializeField] private int _borderSize;

    public void SetupCamera(int boardWidth, int boardHeight)
    {
        SetCameraPosition(boardWidth, boardHeight);
        SetCameraAspectRatio(boardWidth, boardHeight);
    }

    private void SetCameraPosition(int boardWidth, int boardHeight)
    {
        float boardCenterX = (boardWidth - 1) / 2f;
        float boardCenterY = (boardHeight - 1) / 2f;

        _camera.transform.position = new Vector3(boardCenterX, boardCenterY, CameraPositionZ);
    }

    private void SetCameraAspectRatio(int boardWidth, int boardHeight)
    {
        float screenAspectRatio = (float)Screen.width / Screen.height;

        float verticalSize = boardHeight / 2f + _borderSize;
        float horizontalSize = (boardWidth / 2f + _borderSize) / screenAspectRatio;

        _camera.orthographicSize = Mathf.Max(verticalSize, horizontalSize);
    }
}