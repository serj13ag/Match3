using System;
using System.Collections.Generic;
using Data;
using Entities;
using Enums;
using EventArguments;
using Helpers;
using Interfaces;
using Services.Board.States;
using Services.Mono;
using Services.Mono.Sound;
using UnityEngine;

namespace Services.Board
{
    public class BoardService : IUpdatable, IBoardService
    {
        private readonly ITileService _tileService;
        private readonly IGamePieceService _gamePieceService;
        private readonly IScoreService _scoreService;
        private readonly ISoundMonoService _soundMonoService;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IGameRoundService _gameRoundService;

        private readonly string _levelName;

        private Direction _playerSwitchGamePiecesDirection;

        private IBoardState _boardState;

        public Vector2Int BoardSize { get; }

        public event Action OnGamePiecesSwitched;

        public BoardService(string levelName, ISoundMonoService soundMonoService, IUpdateMonoService updateMonoService,
            IPersistentProgressService persistentProgressService, ISaveLoadService saveLoadService,
            IStaticDataService staticDataService, IScoreService scoreService, IGameRoundService gameRoundService,
            ITileService tileService, IGamePieceService gamePieceService)
        {
            _scoreService = scoreService;
            _tileService = tileService;
            _gamePieceService = gamePieceService;
            _soundMonoService = soundMonoService;
            _persistentProgressService = persistentProgressService;
            _saveLoadService = saveLoadService;
            _gameRoundService = gameRoundService;

            _levelName = levelName;

            BoardSize = new Vector2Int(staticDataService.Settings.BoardWidth, staticDataService.Settings.BoardHeight);

            LevelBoardData levelBoardData = persistentProgressService.Progress.BoardData.LevelBoardData;
            if (levelName == levelBoardData.LevelName && levelBoardData.Tiles != null &&
                levelBoardData.GamePieces != null)
            {
                tileService.Initialize(levelBoardData.Tiles);
                gamePieceService.Initialize(levelBoardData.GamePieces);
            }
            else
            {
                tileService.Initialize();
                gamePieceService.Initialize();

                UpdateProgressAndSave();
            }

            updateMonoService.Register(this);

            tileService.OnMoveRequested += OnMoveRequested;

            ChangeStateToWaiting();
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_gameRoundService.RoundIsActive)
            {
                return;
            }

            _boardState.Update(deltaTime);
        }

        public bool PlayerMovedColorBomb(GamePiece clickedGamePiece, GamePiece targetGamePiece,
            out HashSet<GamePiece> gamePiecesToClear)
        {
            gamePiecesToClear = new HashSet<GamePiece>();

            if (targetGamePiece.Color == GamePieceColor.Undefined)
            {
                return false;
            }

            if (clickedGamePiece is BombGamePiece { BombType: BombType.Color })
            {
                if (targetGamePiece is BombGamePiece { BombType: BombType.Color })
                {
                    gamePiecesToClear = _gamePieceService.GetAllGamePieces();
                }
                else
                {
                    gamePiecesToClear = _gamePieceService.GetGamePiecesByColor(targetGamePiece.Color);
                    gamePiecesToClear.Add(clickedGamePiece);
                }

                return true;
            }

            return false;
        }

        public void InvokeGamePiecesSwitched()
        {
            OnGamePiecesSwitched?.Invoke();
        }

        public void ChangeStateToCollapse(HashSet<int> columnIndexesToCollapse)
        {
            ChangeState(new CollapseColumnsTimeoutBoardState(this, _gamePieceService, columnIndexesToCollapse));
        }

        public void ChangeStateToFill()
        {
            ChangeState(new FillTimeoutBoardState(this, _gamePieceService));
        }

        public void ChangeStateToWaiting()
        {
            ChangeState(new WaitingBoardState());
            UpdateProgressAndSave();
        }

        public void ChangeStateToBreak(HashSet<GamePiece> gamePiecesToBreak)
        {
            ChangeState(new BreakGamePiecesTimeoutBoardState(this, _scoreService, _tileService, _gamePieceService,
                _soundMonoService, gamePiecesToBreak));
        }

        private void ChangeStateToHandlePlayerSwitchGamePieces(GamePiece clickedGamePiece, GamePiece targetGamePiece)
        {
            ChangeState(new HandlePlayerSwitchGamePiecesBoardState(this, _gamePieceService, _soundMonoService,
                clickedGamePiece, targetGamePiece));
        }

        private void ChangeState(IBoardState newBoardState)
        {
            _boardState = newBoardState;
        }

        private void OnMoveRequested(object sender, MoveRequestedEventArgs e)
        {
            if (_boardState is WaitingBoardState
                && _gamePieceService.TryGetGamePieceAt(e.FromPosition, out GamePiece clickedGamePiece)
                && !clickedGamePiece.IsCollectible()
                && _gamePieceService.TryGetGamePieceAt(e.ToPosition, out GamePiece targetGamePiece))
            {
                _scoreService.ResetBreakStreakIterations();

                ChangeStateToHandlePlayerSwitchGamePieces(clickedGamePiece, targetGamePiece);
            }
        }

        private void UpdateProgressAndSave()
        {
            _persistentProgressService.Progress.BoardData.LevelBoardData.LevelName = _levelName;

            _tileService.UpdateProgress();
            _gamePieceService.UpdateProgress();

            _saveLoadService.SaveProgress();
        }
    }
}