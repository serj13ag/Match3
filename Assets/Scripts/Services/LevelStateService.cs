﻿using System;
using Enums;
using EventArgs;
using Services.Mono;
using UnityEngine;

namespace Services
{
    public class LevelStateService
    {
        private readonly UiMonoService _uiMonoService;
        private readonly ScoreService _scoreService;
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
                OnMovesLeftChanged?.Invoke(this, new MovesLeftChangedEventArgs(_movesLeft));

                if (_movesLeft == 0)
                {
                    ChangeState(LevelState.GameOver);
                }
            }
        }
        
        public event EventHandler<MovesLeftChangedEventArgs> OnMovesLeftChanged;

        public LevelStateService(UiMonoService uiMonoService, BoardService boardService, ScoreService scoreService,
            SoundMonoService soundMonoService, int scoreGoal, int movesLeft)
        {
            _soundMonoService = soundMonoService;
            _scoreService = scoreService;
            _uiMonoService = uiMonoService;

            boardService.OnGamePiecesSwitched += OnGamePiecesSwitched;
            _scoreService.OnScoreChanged += OnScoreChanged;

            _scoreGoal = scoreGoal;
            _movesLeft = movesLeft;
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

        private void OnScoreChanged(object sender, ScoreChangedEventArgs eventArgs)
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
            return _scoreService.Score >= _scoreGoal;
        }
    }
}