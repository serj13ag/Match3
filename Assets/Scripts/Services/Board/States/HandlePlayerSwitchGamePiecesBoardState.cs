using System;
using System.Collections.Generic;
using System.Linq;
using Constants;
using Entities;
using Enums;
using Helpers;
using Services.Mono.Sound;
using UnityEngine;

namespace Services.Board.States
{
    public class HandlePlayerSwitchGamePiecesBoardState : IBoardState
    {
        private readonly IBoardService _boardService;
        private readonly IGamePieceService _gamePieceService;
        private readonly ISoundMonoService _soundMonoService;
        private readonly Direction _switchGamePiecesDirection;

        private readonly GamePiece[] _movedPieces;
        private int _movedPieceNumber;

        public HandlePlayerSwitchGamePiecesBoardState(IBoardService boardService, IGamePieceService gamePieceService,
            ISoundMonoService soundMonoService, GamePiece clickedGamePiece, GamePiece targetGamePiece)
        {
            _boardService = boardService;
            _gamePieceService = gamePieceService;
            _soundMonoService = soundMonoService;

            _movedPieces = new GamePiece[2];

            clickedGamePiece.OnPositionChanged += OnGamePiecePositionChanged;
            targetGamePiece.OnPositionChanged += OnGamePiecePositionChanged;

            _switchGamePiecesDirection = SwitchGamePieces(clickedGamePiece, targetGamePiece);
        }

        public void Update(float deltaTime)
        {
        }

        private void OnGamePiecePositionChanged(GamePiece gamePiece)
        {
            _movedPieces[_movedPieceNumber] = gamePiece;

            _movedPieceNumber++;
            gamePiece.OnPositionChanged -= OnGamePiecePositionChanged;

            switch (_movedPieceNumber)
            {
                case 2:
                    HandleMovedPieces();
                    break;
                case > 2:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleMovedPieces()
        {
            GamePiece clickedGamePiece = _movedPieces[0];
            GamePiece targetGamePiece = _movedPieces[1];

            if (_boardService.PlayerMovedColorBomb(clickedGamePiece, targetGamePiece,
                    out HashSet<GamePiece> gamePiecesToBreak))
            {
                GamePiecesSwitched(gamePiecesToBreak);
            }
            else if (_gamePieceService.HasMatches(_movedPieces, out HashSet<GamePiece> allMatches))
            {
                gamePiecesToBreak = GetGamePiecesToBreak(allMatches);

                if (allMatches.Count >= Settings.MatchesToSpawnBomb && allMatches.Contains(clickedGamePiece))
                {
                    SpawnBomb(allMatches, clickedGamePiece, gamePiecesToBreak);
                }

                GamePiecesSwitched(gamePiecesToBreak);
            }
            else
            {
                RevertMovedGamePieces(_movedPieces);
                _boardService.ChangeStateToWaiting();
            }
        }

        private void GamePiecesSwitched(HashSet<GamePiece> gamePiecesToBreak)
        {
            _boardService.GamePiecesSwitched();
            _boardService.ChangeStateToBreak(gamePiecesToBreak);
        }

        private static Direction SwitchGamePieces(GamePiece clickedGamePiece, GamePiece targetGamePiece)
        {
            Vector2Int firstGamePiecePosition = clickedGamePiece.Position;
            Vector2Int secondGamePiecePosition = targetGamePiece.Position;

            clickedGamePiece.Move(secondGamePiecePosition);
            targetGamePiece.Move(firstGamePiecePosition);

            return firstGamePiecePosition.x != secondGamePiecePosition.x
                ? Direction.Horizontal
                : Direction.Vertical;
        }

        private void SpawnBomb(HashSet<GamePiece> allMatches, GamePiece clickedGamePiece,
            HashSet<GamePiece> gamePiecesToBreak)
        {
            BombType bombType = GamePieceMatchHelper.GetBombTypeOnMatch(allMatches, _switchGamePiecesDirection);
            _gamePieceService.ClearGamePieceAt(clickedGamePiece.Position);
            _gamePieceService.SpawnBombGamePiece(clickedGamePiece.Position.x, clickedGamePiece.Position.y, bombType,
                clickedGamePiece.Color);

            gamePiecesToBreak.Remove(clickedGamePiece);
        }

        private HashSet<GamePiece> GetGamePiecesToBreak(HashSet<GamePiece> matchedGamePieces)
        {
            HashSet<GamePiece> gamePiecesToBreak = new HashSet<GamePiece>();

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
                BombType.Column => _gamePieceService.GetBombedColumnGamePieces(matchedGamePiece.Position.x),
                BombType.Row => _gamePieceService.GetBombedRowGamePieces(matchedGamePiece.Position.y),
                BombType.Adjacent => _gamePieceService.GetBombedAdjacentGamePieces(matchedGamePiece.Position,
                    Settings.BombAdjacentGamePiecesRange),
                BombType.Color => null,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        private static void RevertMovedGamePieces(GamePiece[] movedGamePieces)
        {
            movedGamePieces[0].Move(movedGamePieces[1].Position);
            movedGamePieces[1].Move(movedGamePieces[0].Position);
        }
    }
}