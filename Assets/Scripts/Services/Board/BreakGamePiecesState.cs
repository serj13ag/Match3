using System.Collections.Generic;
using Constants;
using Entities;
using Enums;
using Services.Mono.Sound;

namespace Services.Board
{
    internal class BreakGamePiecesState : IBoardState
    {
        private const int CompletedBreakIterationsAfterSwitchedGamePieces = 1; // TODO: implement mechanic

        private readonly IBoardService _boardService;
        private readonly IScoreService _scoreService;
        private readonly ITileService _tileService;
        private readonly ISoundMonoService _soundMonoService;

        private readonly HashSet<GamePiece> _gamePiecesToBreak;
        private float _timeTillExecute;

        public BreakGamePiecesState(IBoardService boardService, IScoreService scoreService, ITileService tileService,
            ISoundMonoService soundMonoService, HashSet<GamePiece> gamePiecesToBreak)
        {
            _boardService = boardService;
            _scoreService = scoreService;
            _tileService = tileService;
            _soundMonoService = soundMonoService;

            _gamePiecesToBreak = gamePiecesToBreak;
            _timeTillExecute = Settings.Timeouts.ClearGamePiecesTimeout;
        }

        public void Update(float deltaTime)
        {
            if (_timeTillExecute < 0f)
            {
                BreakGamePieces(_gamePiecesToBreak);
                _boardService.ChangeStateToCollapse(_gamePiecesToBreak);
            }
            else
            {
                _timeTillExecute -= deltaTime;
            }
        }

        public void OnGamePiecePositionChanged(GamePiece gamePiece)
        {
        }

        private void BreakGamePieces(HashSet<GamePiece> gamePieces)
        {
            foreach (GamePiece gamePiece in gamePieces)
            {
                _scoreService.AddScore(gamePiece.Score, gamePieces.Count,
                    CompletedBreakIterationsAfterSwitchedGamePieces);

                _boardService.ClearGamePieceAt(gamePiece.Position, true);
                _tileService.ProcessTileMatchAt(gamePiece.Position);
            }

            _soundMonoService.PlaySound(SoundType.BreakGamePieces);
            if (CompletedBreakIterationsAfterSwitchedGamePieces > 1)
            {
                _soundMonoService.PlaySound(SoundType.Bonus);
            }

            //CompletedBreakIterationsAfterSwitchedGamePieces++;
        }
    }
}