using System;
using Enums;
using UnityEngine;

namespace Controllers
{
    public class GameStateController : MonoBehaviour
    {
        private Board _board;
        private CameraService _cameraService;
        private UIController _uiController;
        private ScoreController _scoreController;
        private SoundController _soundController;

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

        public void Init(UIController uiController, Board board, CameraService cameraService,
            ScoreController scoreController, SoundController soundController)
        {
            _soundController = soundController;
            _scoreController = scoreController;
            _uiController = uiController;
            _cameraService = cameraService;
            _board = board;

            _board.OnGamePiecesSwitched += OnGamePiecesSwitched;
            _scoreController.OnScoreChanged += OnScoreChanged;
        }

        public void InitializeLevel(int movesLeft, int scoreGoal)
        {
            _movesLeft = movesLeft;
            _scoreGoal = scoreGoal;

            ChangeState(GameState.Initialization);
        }

        private void ChangeStateToPlaying()
        {
            ChangeState(GameState.Playing);
        }

        private void ChangeState(GameState state)
        {
            _gameState = state;

            switch (state)
            {
                case GameState.Initialization:
                {
                    _board.SetupTiles();
                    _cameraService.SetupCamera(_board.BoardSize);
                    _board.SetupGamePieces();

                    UpdateMovesLeftText();

                    _soundController.PlaySound(SoundType.Music);
                    _uiController.ShowStartGameMessageWindow(_scoreGoal, ChangeStateToPlaying);
                    break;
                }
                case GameState.Playing:
                {
                    _uiController.FadeOff();
                    break;
                }
                case GameState.GameOver:
                {
                    _uiController.FadeOn();

                    if (ScoreGoalReached())
                    {
                        _soundController.PlaySound(SoundType.Win);
                        _uiController.ShowGameWinMessageWindow(ReloadLevel);
                    }
                    else
                    {
                        _soundController.PlaySound(SoundType.Lose);
                        _uiController.ShowGameOverMessageWindow(ReloadLevel);
                    }

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void ReloadLevel()
        {
            //AllServices.Instance.GameStateMachine.ReloadLevel(); //TODO
        }

        private void OnGamePiecesSwitched()
        {
            MovesLeft--;
        }

        private void OnScoreChanged()
        {
            if (_gameState == GameState.GameOver)
            {
                return;
            }

            if (ScoreGoalReached())
            {
                ChangeState(GameState.GameOver);
            }
        }

        private bool ScoreGoalReached()
        {
            return _scoreController.Score >= _scoreGoal;
        }

        private void UpdateMovesLeftText()
        {
            //_movesLeftText.text = _movesLeft.ToString(); //TODO
        }
    }
}