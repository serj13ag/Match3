using System;
using Data;

namespace Services
{
    public interface IPersistentDataService : IService
    {
        PlayerProgress Progress { get; }
        GameSettings GameSettings { get; }
        Purchases Purchases { get; }
        Customizations Customizations { get; }

        void InitData(PlayerData savedPlayerData);
        void ResetProgressAndSave();
        void Save();
        event EventHandler<EventArgs> OnResetProgress;
    }
}