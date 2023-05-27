using System;
using Enums;
using Services.Mono;
using UnityEngine;

namespace Services
{
    public class LevelStateService
    {
        private readonly UIController _uiController;
        private readonly ScoreController _scoreController;
        private readonly SoundController _soundController;

        private LevelState _levelState;

        private int _movesLeft;
        private readonly int _scoreGoal;

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
                    Debug.LogError($"{nameof(LevelStateService)} : Attempt to set {nameof(MovesLeft)} a negative value!");
                    _movesLeft = 0;
                }

                _movesLeft = value;
                UpdateMovesLeftText();

                if (_movesLeft == 0)
                {
                    ChangeState(LevelState.GameOver);
                }
            }
        }

        public LevelStateService(UIController uiController, BoardService boardService, ScoreController scoreController,
            SoundController soundController, int scoreGoal, int movesLeft)
        {
            _soundController = soundController;
            _scoreController = scoreController;
            _uiController = uiController;

            boardService.OnGamePiecesSwitched += OnGamePiecesSwitched;
            _scoreController.OnScoreChanged += OnScoreChanged;

            _scoreGoal = scoreGoal;
            _movesLeft = movesLeft;
            
            UpdateMovesLeftText();
        }

        public void ChangeStateToPlaying()
        {
            ChangeState(LevelState.Playing);
        }

        private void ChangeState(LevelState state)
        {
            _levelState = state;

            switch (state)
            {
                case LevelState.Playing:
                {
                    _uiController.FadeOff();
                    break;
                }
                case LevelState.GameOver:
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
            if (_levelState == LevelState.GameOver)
            {
                return;
            }

            if (ScoreGoalReached())
            {
                ChangeState(LevelState.GameOver);
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