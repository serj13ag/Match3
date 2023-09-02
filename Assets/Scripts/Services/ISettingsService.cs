﻿using System;
using EventArguments;

namespace Services
{
    public interface ISettingsService : IService
    {
        bool MusicEnabled { get; }
        bool SoundEnabled { get; }

        event EventHandler<SettingsChangedEventArgs> OnSettingsChanged;

        void MusicSetActive(bool activate);
        void SoundSetActive(bool activate);
    }
}