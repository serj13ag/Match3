using System;
using Constants;
using Entities;
using Enums;

namespace Services.Board.States
{
    public class WaitingBoardState : IBoardState
    {
        private readonly IParticleService _particleService;

        private readonly Tuple<GamePiece, GamePiece> _gamePiecesForMatchHint;
        private float _timeTillShowHint;

        public WaitingBoardState(IBoardService boardService, IGamePieceService gamePieceService,
            IParticleService particleService)
        {
            _particleService = particleService;
            if (!gamePieceService.HasAvailableMoves(out Tuple<GamePiece, GamePiece> gamePiecesForMatch))
            {
                gamePieceService.ClearBoard();
                boardService.ChangeStateToFill();
            }

            _gamePiecesForMatchHint = gamePiecesForMatch;
            _timeTillShowHint = Settings.SecondsTillShowHint;
        }

        public void Update(float deltaTime)
        {
            if (_timeTillShowHint < 0)
            {
                PlayHints();
                _timeTillShowHint = Settings.SecondsBetweenReshowHint;
            }
            else
            {
                _timeTillShowHint -= deltaTime;
            }
        }

        private void PlayHints()
        {
            _particleService.PlayParticleEffectAt(_gamePiecesForMatchHint.Item1.Position, ParticleEffectType.Hint);
            _particleService.PlayParticleEffectAt(_gamePiecesForMatchHint.Item2.Position, ParticleEffectType.Hint);
        }
    }
}