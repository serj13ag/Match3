using System;
using Data;

namespace EventArguments
{
    public class SettingsChangedEventArgs : EventArgs
    {
        public GameSettings GameSettings { get; }

        public SettingsChangedEventArgs(GameSettings gameSettings)
        {
            GameSettings = gameSettings;
        }
    }
}