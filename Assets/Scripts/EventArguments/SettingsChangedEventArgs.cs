using System;
using Data;

namespace EventArguments
{
    public class SettingsChangedEventArgs : EventArgs
    {
        public bool SoundEnabled { get; }

        public SettingsChangedEventArgs(GameSettings gameSettings)
        {
            SoundEnabled = gameSettings.SoundEnabled;
        }
    }
}