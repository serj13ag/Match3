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
    public class BoardService : IUpdatable, IBoardService, IProgressWriter
    {
        private readonly ITileService _tileService;
        private readonly IGamePieceService _gamePieceService;
        private readonly IScoreService _scoreService;
        private readonly IMovesLeftService _movesLeftService;
        private readonly ISoundMonoService _soundMonoService;
        private readonly IProgressUpdateService _progressUpdateService;
        private readonly IGameRoundService _gameRoundService;

        private readonly string _levelName;

        private Direction _playerSwitchGamePiecesDirection;

        private IBoardState _boardState;

        public Vector2Int BoardSize { get; }

        public BoardService(string levelName, ISoundMonoService soundMonoService, IUpdateMonoService updateMonoService,
            IPersistentProgressService persistentProgressService, IStaticDataService staticDataService,
            IProgressUpdateService progressUpdateService, IScoreService scoreService,
            IMovesLeftService movesLeftService, IGameRoundService gameRoundService,
            ITileService tileService, IGamePieceService gamePieceService)
        {
            _scoreService = scoreService;
            _movesLeftService = movesLeftService;
            _tileService = tileService;
            _gamePieceService = gamePieceService;
            _soundMonoService = soundMonoService;
            _progressUpdateService = progressUpdateService;
            _gameRoundService = gameRoundService;

            _levelName = levelName;

            BoardSize = new Vector2Int(staticDataService.Settings.BoardWidth, staticDataService.Settings.BoardHeight);

            updateMonoService.Register(this);
            progressUpdateService.Register(this);

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
            }

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

        public void WriteToProgress(PlayerProgress progress)
        {
            progress.BoardData.LevelBoardData.LevelName = _levelName;
        }

        public bool PlayerMovedColorBomb(GamePiece clickedGamePiece, GamePiece targetGamePiece,
            out HashSet<GamePiece> gamePiecesToBreak)
        {
            gamePiecesToBreak = new HashSet<GamePiece>();

            if (targetGamePiece.Color == GamePieceColor.Undefined)
            {
                return false;
            }

            if (clickedGamePiece is BombGamePiece { BombType: BombType.Color })
            {
                if (targetGamePiece is BombGamePiece { BombType: BombType.Color })
                {
                    gamePiecesToBreak = _gamePieceService.GetAllGamePieces();
                }
                else
                {
                    gamePiecesToBreak = _gamePieceService.GetGamePiecesByColor(targetGamePiece.Color);
                    gamePiecesToBreak.Add(clickedGamePiece);
                }

                return true;
            }

            return false;
        }

        public void GamePiecesSwitched()
        {
            _movesLeftService.DecrementMovesLeft();
        }

        public void ChangeStateToCollapse(HashSet<int> columnIndexesToCollapse)
        {
            ChangeState(new CollapseColumnsTimeoutBoardState(this, _gamePieceService, _gameRoundService, _movesLeftService, columnIndexesToCollapse));
        }

        public void ChangeStateToFill()
        {
            ChangeState(new FillTimeoutBoardState(this, _gamePieceService));
        }

        public void ChangeStateToWaiting()
        {
            ChangeState(new WaitingBoardState());

            _progressUpdateService.UpdateProgressAndSave();
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
    }
}