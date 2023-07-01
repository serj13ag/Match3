using System;

namespace Data
{
    [Serializable]
    public class PlayerProgress
    {
        public BoardData BoardData;

        public PlayerProgress()
        {
            BoardData = new BoardData();
        }
    }
}