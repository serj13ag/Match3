using System.Collections.Generic;
using Constants;
using Entities;
using Enums;
using Helpers;
using Services.Mono.Sound;

namespace Services.Board.States
{
    public class BreakGamePiecesTimeoutBoardState : BaseTimeoutBoardState
    {
        private readonly IBoardService _boardService;
        private readonly IScoreService _scoreService;
        private readonly ITileService _tileService;
        private readonly IGamePieceService _gamePieceService;
        private readonly ISoundMonoService _soundMonoService;

        private readonly HashSet<GamePiece> _gamePiecesToBreak;

        public BreakGamePiecesTimeoutBoardState(IBoardService boardService, IScoreService scoreService,
            ITileService tileService, IGamePieceService gamePieceService, ISoundMonoService soundMonoService,
            HashSet<GamePiece> gamePiecesToBreak)
            : base(Settings.Timeouts.ClearGamePiecesTimeout)
        {
            _boardService = boardService;
            _scoreService = scoreService;
            _tileService = tileService;
            _gamePieceService = gamePieceService;
            _soundMonoService = soundMonoService;

            _gamePiecesToBreak = gamePiecesToBreak;
        }

        protected override void OnTimeoutEnded()
        {
            BreakGamePieces(_gamePiecesToBreak);

            _boardService.ChangeStateToCollapse(BoardHelper.GetColumnIndexes(_gamePiecesToBreak));
        }

        private void BreakGamePieces(HashSet<GamePiece> gamePieces)
        {
            bool hasBombGamePieces = false;

            foreach (GamePiece gamePiece in gamePieces)
            {
                if (gamePiece is BombGamePiece)
                {
                    hasBombGamePieces = true;
                }

                _scoreService.AddScore(gamePiece.Score, gamePieces.Count);

                _gamePieceService.ClearGamePieceAt(gamePiece.Position, true);
                _tileService.ProcessTileMatchAt(gamePiece.Position);
            }

            _soundMonoService.PlaySound(hasBombGamePieces ? SoundType.BombGamePieces : SoundType.BreakGamePieces);

            _scoreService.IncrementCompletedBreakStreakIterations();
        }
    }
}