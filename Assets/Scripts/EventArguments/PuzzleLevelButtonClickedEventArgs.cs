using System;

namespace EventArguments
{
    public class PuzzleLevelButtonClickedEventArgs : EventArgs
    {
        public string LevelName { get; }

        public PuzzleLevelButtonClickedEventArgs(string levelName)
        {
            LevelName = levelName;
        }
    }
}