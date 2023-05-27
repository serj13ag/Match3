using System;

namespace Data
{
    [Serializable]
    public class PlayerProgress
    {
        public BoardData BoardData;

        public PlayerProgress(string levelName)
        {
            BoardData = new BoardData(levelName);
        }
    }
}