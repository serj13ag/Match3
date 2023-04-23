using Enums;
using UnityEngine;

namespace Controllers
{
    public class GameStateController : MonoBehaviour
    {
        private Board _board;
        private CameraController _cameraController;

        private GameState _gameState;

        private int _movesLeft = 30;
        private int _scoreGoal = 10000;

        public void Init(Board board, CameraController cameraController)
        {
            _cameraController = cameraController;
            _board = board;
        }

        public void StartGame()
        {
            _gameState = GameState.Initialization;

            _board.SetupTiles();
            _cameraController.SetupCamera(_board.BoardSize);
            _board.SetupGamePieces();
        }
    }
}