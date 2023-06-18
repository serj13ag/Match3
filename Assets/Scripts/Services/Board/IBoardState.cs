using Entities;

namespace Services.Board
{
    public interface IBoardState
    {
        void Update(float deltaTime);
        void OnGamePiecePositionChanged(GamePiece gamePiece);
    }
}