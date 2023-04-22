namespace Entities.Tiles
{
    public class ObstacleTile : BaseTile
    {
        public override bool IsObstacle => true;

        public override void ProcessMatch()
        {
        }
    }
}