using System;
using Enums;
using Services.Mono;
using UnityEngine;

namespace Services
{
    public class LevelStateService
    {
        private readonly UiMonoService _uiMonoService;
        private readonly ScoreMonoService _scoreMonoService;
        private readonly SoundMonoService _soundMonoService;

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

        public LevelStateService(UiMonoService uiMonoService, BoardService boardService, ScoreMonoService scoreMonoService,
            SoundMonoService soundMonoService, int scoreGoal, int movesLeft)
        {
            _soundMonoService = soundMonoService;
            _scoreMonoService = scoreMonoService;
            _uiMonoService = uiMonoService;

            boardService.OnGamePiecesSwitched += OnGamePiecesSwitched;
            _scoreMonoService.OnScoreChanged += OnScoreChanged;

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
                    _uiMonoService.FadeOff();
                    break;
                }
                case LevelState.GameOver:
                {
                    _uiMonoService.FadeOn();

                    if (ScoreGoalReached())
                    {
                        _soundMonoService.PlaySound(SoundType.Win);
                        _uiMonoService.ShowGameWinMessageWindow(ReloadLevel);
                    }
                    else
                    {
                        _soundMonoService.PlaySound(SoundType.Lose);
                        _uiMonoService.ShowGameOverMessageWindow(ReloadLevel);
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
            return _scoreMonoService.Score >= _scoreGoal;
        }

        private void UpdateMovesLeftText()
        {
            //_movesLeftText.text = _movesLeft.ToString(); //TODO
        }
    }
}