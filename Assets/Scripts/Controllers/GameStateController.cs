using System;
using Enums;
using TMPro;
using UnityEngine;

namespace Controllers
{
    public class GameStateController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _movesLeftText;

        private Board _board;
        private CameraController _cameraController;
        private ScreenFaderController _screenFaderController;

        private GameState _gameState;

        private int _movesLeft;
        private int _scoreGoal;

        public int MovesLeft
        {
            get => _movesLeft;
            set
            {
                if (_movesLeft == value)
                {
                    return;
                }

                if (_movesLeft < 0)
                {
                    Debug.LogError(
                        $"{nameof(GameStateController)} : Attempt to set {nameof(MovesLeft)} a negative value!");
                    _movesLeft = 0;
                }

                _movesLeft = value;
                UpdateMovesLeftText();

                if (_movesLeft == 0)
                {
                    ChangeState(GameState.GameOver);
                }
            }
        }

        public void Init(ScreenFaderController screenFaderController, Board board, CameraController cameraController)
        {
            _screenFaderController = screenFaderController;
            _cameraController = cameraController;
            _board = board;

            _movesLeft = 3;
            _scoreGoal = 10000;

            _board.OnGamePiecesSwitched += OnGamePiecesSwitched;
        }

        public void StartGame()
        {
            ChangeState(GameState.Initialization);

            _board.SetupTiles();
            _cameraController.SetupCamera(_board.BoardSize);
            _board.SetupGamePieces();

            UpdateMovesLeftText();

            ChangeState(GameState.Playing);
        }

        private void ChangeState(GameState state)
        {
            _gameState = state;

            switch (state)
            {
                case GameState.Initialization:
                    break;
                case GameState.Playing:
                    break;
                case GameState.GameOver:
                    _screenFaderController.FadeOn();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void OnGamePiecesSwitched()
        {
            MovesLeft--;
        }

        private void UpdateMovesLeftText()
        {
            _movesLeftText.text = _movesLeft.ToString();
        }
    }
}