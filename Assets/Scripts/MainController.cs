using UnityEngine;

public class MainController : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private Board _board;

    private void Start()
    {
        _board.SetupTiles();
        _cameraController.SetupCamera(_board.Width, _board.Height);
    }
}