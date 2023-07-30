using System;

namespace Data
{
    [Serializable]
    public class PlayerProgress
    {
        public int PlayerLevel;
        public BoardData BoardData;

        public PlayerProgress()
        {
            PlayerLevel = 1;
            BoardData = new BoardData();
        }
    }
}