using System;

namespace EventArguments
{
    public class PlayerLevelChangedEventArgs : EventArgs
    {
        public int Level { get; }

        public PlayerLevelChangedEventArgs(int level)
        {
            Level = level;
        }
    }
}