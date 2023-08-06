using System.Collections.Generic;
using Data;
using DTO;
using Entities;
using Enums;
using UnityEngine;

namespace Services
{
    public interface IGamePieceService
    {
        void Initialize();
        void Initialize(List<GamePieceSaveData> gamePieces);

        List<GamePiece> FillBoardWithRandomGamePieces(int offsetY = 0);
        void ClearBoard();
        void ClearGamePieceAt(Vector2Int position, bool breakOnMatch = false);
        void SpawnBombGamePiece(int x, int y, BombType bombType, GamePieceColor color);
        bool TryGetGamePieceAt(Vector2Int position, out GamePiece gamePiece);
        bool HasMatches(IEnumerable<GamePiece> gamePieces, out HashSet<GamePiece> allMatches);
        bool HasCollectiblesToBreak(out HashSet<GamePiece> collectiblesToBreak);
        IEnumerable<GamePieceMoveData> GetGamePiecesToCollapseMoveData(int column);

        HashSet<GamePiece> GetBombedRowGamePieces(int row);
        HashSet<GamePiece> GetBombedColumnGamePieces(int column);
        HashSet<GamePiece> GetBombedAdjacentGamePieces(Vector2Int position, int range);
        HashSet<GamePiece> GetGamePiecesByColor(GamePieceColor color);
        HashSet<GamePiece> GetAllGamePieces();

        bool TryGetLowestRowWithEmptyGamePiece(out int lowestEmptyRow);
        bool HasAvailableMoves(out GamePiece[] gamePiecesForMatch);
    }
}