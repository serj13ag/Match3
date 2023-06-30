using System;

namespace EventArguments
{
    public class LevelButtonClickedEventArgs : EventArgs
    {
        public string LevelName { get; }

        public LevelButtonClickedEventArgs(string levelName)
        {
            LevelName = levelName;
        }
    }
}