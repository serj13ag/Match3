using System.Collections.Generic;
using Data;
using Entities;
using Enums;
using EventArguments;
using Helpers;
using Interfaces;
using Services.Board.States;
using Services.GameRound;
using Services.Mono;
using Services.Mono.Sound;
using Services.MovesLeft;
using UnityEngine;

namespace Services.Board
{
    public class BoardService : IUpdatable, IBoardService
    {
        private readonly ITileService _tileService;
        private readonly IGamePieceService _gamePieceService;
        private readonly IParticleService _particleService;
        private readonly IScoreService _scoreService;
        private readonly IMovesLeftService _movesLeftService;
        private readonly ISoundMonoService _soundMonoService;
        private readonly IUpdateMonoService _updateMonoService;
        private readonly IProgressUpdateService _progressUpdateService;
        private readonly IGameRoundService _gameRoundService;

        private Direction _playerSwitchGamePiecesDirection;

        private IBoardState _boardState;

        public Vector2Int BoardSize { get; }

        public BoardService(string levelName, ISoundMonoService soundMonoService, IUpdateMonoService updateMonoService,
            IPersistentDataService persistentDataService, IStaticDataService staticDataService,
            IProgressUpdateService progressUpdateService, IScoreService scoreService,
            IMovesLeftService movesLeftService, IGameRoundService gameRoundService,
            ITileService tileService, IGamePieceService gamePieceService, IParticleService particleService)
        {
            _scoreService = scoreService;
            _movesLeftService = movesLeftService;
            _tileService = tileService;
            _gamePieceService = gamePieceService;
            _particleService = particleService;
            _soundMonoService = soundMonoService;
            _updateMonoService = updateMonoService;
            _progressUpdateService = progressUpdateService;
            _gameRoundService = gameRoundService;

            BoardSize = new Vector2Int(staticDataService.Settings.BoardWidth, staticDataService.Settings.BoardHeight);

            updateMonoService.Register(this);

            if (persistentDataService.Progress.BoardData.TryGetValue(levelName, out LevelBoardData levelBoardData))
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

            ChangeStateToWaiting(true);
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
            out HashSet<GamePiece> gamePiecesToBreak)
        {
            gamePiecesToBreak = new HashSet<GamePiece>();

            if (clickedGamePiece is not BombGamePiece bombGamePiece || bombGamePiece.BombType != BombType.Color)
            {
                return false;
            }

            if (targetGamePiece is BombGamePiece { BombType: BombType.Color })
            {
                gamePiecesToBreak = _gamePieceService.GetAllGamePieces();
            }
            else
            {
                gamePiecesToBreak = _gamePieceService.GetGamePiecesByColor(targetGamePiece.Color);
                gamePiecesToBreak = GamePieceMatchHelper.GetGamePiecesToBreak(gamePiecesToBreak, _gamePieceService);
                gamePiecesToBreak.Add(clickedGamePiece);
            }

            return gamePiecesToBreak.Count > 0;
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

        public void ChangeStateToWaiting(bool needSave = false)
        {
            ChangeState(new WaitingBoardState(this, _gamePieceService, _particleService));

            if (needSave)
            {
                _progressUpdateService.UpdateProgressAndSave();
            }
        }

        public void ChangeStateToBreak(HashSet<GamePiece> gamePiecesToBreak)
        {
            ChangeState(new BreakGamePiecesTimeoutBoardState(this, _scoreService, _tileService, _gamePieceService,
                _soundMonoService, gamePiecesToBreak));
        }

        private void ChangeStateToHandlePlayerSwitchGamePieces(GamePiece clickedGamePiece, GamePiece targetGamePiece)
        {
            ChangeState(new HandlePlayerSwitchGamePiecesBoardState(this, _gamePieceService, clickedGamePiece,
                targetGamePiece));
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

        public void Cleanup()
        {
            _boardState = null;

            _updateMonoService.Remove(this);
        }
    }
}