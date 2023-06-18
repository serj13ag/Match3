using System;
using System.Collections.Generic;
using System.Linq;
using Constants;
using Entities;
using Enums;
using Helpers;
using Services.Board;
using Services.Mono.Sound;

namespace Services
{
    public class HandlePlayerSwitchGamePiecesBoardState : IBoardState
    {
        private readonly IBoardService _boardService;
        private readonly ISoundMonoService _soundMonoService;
        private readonly Direction _playerSwitchGamePiecesDirection;

        private readonly Stack<GamePiece> _movedPieces;

        public HandlePlayerSwitchGamePiecesBoardState(IBoardService boardService, ISoundMonoService soundMonoService,
            Direction playerSwitchGamePiecesDirection)
        {
            _boardService = boardService;
            _soundMonoService = soundMonoService;
            _playerSwitchGamePiecesDirection = playerSwitchGamePiecesDirection;

            _movedPieces = new Stack<GamePiece>();
        }

        public void Update(float deltaTime)
        {
        }

        public void OnGamePiecePositionChanged(GamePiece gamePiece)
        {
            _movedPieces.Push(gamePiece);

            if (_movedPieces.Count == 2)
            {
                GamePiece[] movedGamePieces =
                {
                    _movedPieces.Pop(),
                    _movedPieces.Pop(),
                };

                if (_boardService.PlayerMovedColorBomb(movedGamePieces[1], movedGamePieces[0],
                        out HashSet<GamePiece> gamePiecesToClear))
                {
                    _boardService.InvokeGamePiecesSwitched(); // TODO one invocation

                    _boardService.ChangeStateToBreak(gamePiecesToClear);
                }
                else if (_boardService.HasMatches(movedGamePieces, out HashSet<GamePiece> allMatches))
                {
                    _boardService.InvokeGamePiecesSwitched(); // TODO one invocation

                    HashSet<GamePiece> gamePiecesToBreak = GetGamePiecesToBreak(allMatches);

                    GamePiece clickedGamePiece = movedGamePieces[1];
                    if (allMatches.Count >= Settings.MatchesToSpawnBomb && allMatches.Contains(clickedGamePiece))
                    {
                        SpawnBomb(allMatches, clickedGamePiece, gamePiecesToBreak);
                    }

                    _boardService.ChangeStateToBreak(gamePiecesToBreak);
                }
                else
                {
                    RevertMovedGamePieces(movedGamePieces);
                    _boardService.ChangeStateToWaiting();
                }
            }
        }

        private void SpawnBomb(HashSet<GamePiece> allMatches, GamePiece clickedGamePiece,
            HashSet<GamePiece> gamePiecesToBreak)
        {
            BombType bombType = GamePieceMatchHelper.GetBombTypeOnMatch(allMatches, _playerSwitchGamePiecesDirection);
            _boardService.ClearGamePieceAt(clickedGamePiece.Position);
            _boardService.SpawnBombGamePiece(clickedGamePiece.Position.x, clickedGamePiece.Position.y, bombType,
                clickedGamePiece.Color);

            gamePiecesToBreak.Remove(clickedGamePiece);
        }

        private HashSet<GamePiece> GetGamePiecesToBreak(HashSet<GamePiece> matchedGamePieces)
        {
            var gamePiecesToBreak = new HashSet<GamePiece>();

            foreach (GamePiece matchedGamePiece in matchedGamePieces)
            {
                if (TryGetBombedGamePieces(matchedGamePiece, out HashSet<GamePiece> bombedGamePieces))
                {
                    _soundMonoService.PlaySound(SoundType.BombGamePieces);
                    gamePiecesToBreak.UnionWith(bombedGamePieces);
                }
                else
                {
                    gamePiecesToBreak.Add(matchedGamePiece);
                }
            }

            return gamePiecesToBreak;
        }

        private bool TryGetBombedGamePieces(GamePiece matchedGamePiece, out HashSet<GamePiece> bombedGamePieces,
            HashSet<GamePiece> gamePiecesToExclude = null)
        {
            bombedGamePieces = new HashSet<GamePiece>();

            if (matchedGamePiece is not BombGamePiece bombGamePiece)
            {
                return false;
            }

            bombedGamePieces = GetBombedGamePieces(bombGamePiece.BombType, matchedGamePiece);

            if (bombedGamePieces == null)
            {
                return false;
            }

            // TODO: fix later
            foreach (GamePiece bombedGamePiece in bombedGamePieces)
            {
                bombedGamePiece.Bombed = true;
            }

            if (gamePiecesToExclude != null)
            {
                bombedGamePieces.ExceptWith(gamePiecesToExclude);
            }

            foreach (var bombedGamePiece in bombedGamePieces.ToArray())
            {
                if (TryGetBombedGamePieces(bombedGamePiece, out var pieces, bombedGamePieces))
                {
                    bombedGamePieces.UnionWith(pieces);
                }
            }

            return true;
        }

        private HashSet<GamePiece> GetBombedGamePieces(BombType bombType, GamePiece matchedGamePiece)
        {
            return bombType switch
            {
                BombType.Column => _boardService.GetBombedColumnGamePieces(matchedGamePiece.Position.x),
                BombType.Row => _boardService.GetBombedRowGamePieces(matchedGamePiece.Position.y),
                BombType.Adjacent => _boardService.GetBombedAdjacentGamePieces(matchedGamePiece.Position,
                    Settings.BombAdjacentGamePiecesRange),
                BombType.Color => null,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        private void RevertMovedGamePieces(GamePiece[] movedGamePieces)
        {
            movedGamePieces[0].Move(movedGamePieces[1].Position);
            movedGamePieces[1].Move(movedGamePieces[0].Position);
        }
    }
}