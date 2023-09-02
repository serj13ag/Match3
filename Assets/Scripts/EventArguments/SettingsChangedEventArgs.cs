using System;
using Data;

namespace EventArguments
{
    public class SettingsChangedEventArgs : EventArgs
    {
        public bool MusicEnabled { get; }
        public bool SoundEnabled { get; }

        public SettingsChangedEventArgs(GameSettings gameSettings)
        {
            MusicEnabled = gameSettings.MusicEnabled;
            SoundEnabled = gameSettings.SoundEnabled;
        }
    }
}