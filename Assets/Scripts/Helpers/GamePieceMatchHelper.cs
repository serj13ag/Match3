using System.Collections.Generic;
using Entities;
using Enums;
using UnityEngine;

namespace Helpers
{
    public static class GamePieceMatchHelper
    {
        public static BombType GetBombTypeOnMatch(HashSet<GamePiece> allMatches, Direction playerSwitchGamePiecesDirection)
        {
            BombType bombType;
            if (IsCornerMatch(allMatches))
            {
                bombType = BombType.Adjacent;
            }
            else
            {
                bombType = playerSwitchGamePiecesDirection == Direction.Vertical
                    ? BombType.Column
                    : BombType.Row;
            }

            return bombType;
        }

        public static bool HasMatchAtFillBoard(Vector2Int position, GamePiece[,] gamePieces, Vector2Int boardSize)
        {
            if (TryFindMatchesByDirection(position, Vector2Int.left, Constants.MinMatchesCount, gamePieces,
                    boardSize, out _))
            {
                return true;
            }

            if (TryFindMatchesByDirection(position, Vector2Int.down, Constants.MinMatchesCount, gamePieces,
                    boardSize, out _))
            {
                return true;
            }

            return false;
        }

        public static bool TryFindMatches(Vector2Int startPosition, int minMatchesCount, GamePiece[,] gamePieces,
            Vector2Int boardSize, out HashSet<GamePiece> matches)
        {
            matches = new HashSet<GamePiece>();

            if (TryFindHorizontalMatches(startPosition, minMatchesCount,
                    gamePieces, boardSize, out HashSet<GamePiece> horizontalMatches))
            {
                matches.UnionWith(horizontalMatches);
            }

            if (TryFindVerticalMatches(startPosition, minMatchesCount,
                    gamePieces, boardSize, out HashSet<GamePiece> verticalMatches))
            {
                matches.UnionWith(verticalMatches);
            }

            return matches.Count >= minMatchesCount;
        }

        private static bool TryFindHorizontalMatches(Vector2Int startPosition, int minMatchesCount,
            GamePiece[,] gamePieces, Vector2Int boardSize, out HashSet<GamePiece> horizontalMatches)
        {
            horizontalMatches = new HashSet<GamePiece>();

            if (TryFindMatchesByDirection(startPosition, Vector2Int.right, 2, gamePieces,
                    boardSize, out HashSet<GamePiece> upMatches))
            {
                horizontalMatches.UnionWith(upMatches);
            }

            if (TryFindMatchesByDirection(startPosition, Vector2Int.left, 2, gamePieces,
                    boardSize, out HashSet<GamePiece> downMatches))
            {
                horizontalMatches.UnionWith(downMatches);
            }

            return horizontalMatches.Count >= minMatchesCount;
        }

        private static bool TryFindVerticalMatches(Vector2Int startPosition, int minMatchesCount,
            GamePiece[,] gamePieces, Vector2Int boardSize, out HashSet<GamePiece> verticalMatches)
        {
            verticalMatches = new HashSet<GamePiece>();

            if (TryFindMatchesByDirection(startPosition, Vector2Int.up, 2, gamePieces,
                    boardSize, out HashSet<GamePiece> upMatches))
            {
                verticalMatches.UnionWith(upMatches);
            }

            if (TryFindMatchesByDirection(startPosition, Vector2Int.down, 2, gamePieces,
                    boardSize, out HashSet<GamePiece> downMatches))
            {
                verticalMatches.UnionWith(downMatches);
            }

            return verticalMatches.Count >= minMatchesCount;
        }

        private static bool TryFindMatchesByDirection(Vector2Int startPosition, Vector2Int searchDirection,
            int minMatchesCount, GamePiece[,] gamePieces, Vector2Int boardSize, out HashSet<GamePiece> matches)
        {
            matches = new HashSet<GamePiece>();

            GamePiece startGamePiece = gamePieces[startPosition.x, startPosition.y];
            matches.Add(startGamePiece);

            Vector2Int nextPosition = Vector2Int.zero;

            int maxSearches = Mathf.Max(boardSize.x, boardSize.y);
            for (var i = 0; i < maxSearches - 1; i++)
            {
                nextPosition.x = startPosition.x + searchDirection.x * i;
                nextPosition.y = startPosition.y + searchDirection.y * i;

                if (BoardHelper.IsOutOfBounds(nextPosition, boardSize))
                {
                    break;
                }

                GamePiece gamePieceToCheck = gamePieces[nextPosition.x, nextPosition.y];

                if (gamePieceToCheck == null)
                {
                    break;
                }

                if (gamePieceToCheck.Color != startGamePiece.Color)
                {
                    break;
                }

                matches.Add(gamePieceToCheck);
            }

            return matches.Count >= minMatchesCount;
        }

        private static bool IsCornerMatch(HashSet<GamePiece> gamePieces)
        {
            var horizontalMatches = false;
            var verticalMatches = false;

            var isFirstGamePiece = true;
            var firstGamePiecePosition = Vector2Int.zero;

            foreach (var gamePiece in gamePieces)
            {
                if (isFirstGamePiece)
                {
                    firstGamePiecePosition = gamePiece.Position;
                    isFirstGamePiece = false;
                }

                if (firstGamePiecePosition.x != gamePiece.Position.x
                    && firstGamePiecePosition.y == gamePiece.Position.y)
                {
                    horizontalMatches = true;
                }

                if (firstGamePiecePosition.x == gamePiece.Position.x
                    && firstGamePiecePosition.y != gamePiece.Position.y)
                {
                    verticalMatches = true;
                }
            }

            return horizontalMatches && verticalMatches;
        }
    }
}