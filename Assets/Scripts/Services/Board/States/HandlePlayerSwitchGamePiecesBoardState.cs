using System;
using System.Collections.Generic;
using Constants;
using Entities;
using Enums;
using Helpers;
using UnityEngine;

namespace Services.Board.States
{
    public class HandlePlayerSwitchGamePiecesBoardState : IBoardState
    {
        private readonly IBoardService _boardService;
        private readonly IGamePieceService _gamePieceService;
        private readonly Direction _switchGamePiecesDirection;

        private readonly GamePiece[] _movedPieces;
        private int _movedPieceNumber;

        public HandlePlayerSwitchGamePiecesBoardState(IBoardService boardService, IGamePieceService gamePieceService,
            GamePiece clickedGamePiece, GamePiece targetGamePiece)
        {
            _boardService = boardService;
            _gamePieceService = gamePieceService;

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
                gamePiecesToBreak = GamePieceMatchHelper.GetGamePiecesToBreak(allMatches, _gamePieceService);

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
            _boardService.ChangeStateToBreak(gamePiecesToBreak);
        }

        private static Direction SwitchGamePieces(GamePiece clickedGamePiece, GamePiece targetGamePiece)
        {
            Vector2Int firstGamePiecePosition = clickedGamePiece.Position;
            Vector2Int secondGamePiecePosition = targetGamePiece.Position;

            clickedGamePiece.Move(secondGamePiecePosition, true);
            targetGamePiece.Move(firstGamePiecePosition, true);

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

        private static void RevertMovedGamePieces(GamePiece[] movedGamePieces)
        {
            movedGamePieces[0].Move(movedGamePieces[1].Position);
            movedGamePieces[1].Move(movedGamePieces[0].Position);
        }
    }
}