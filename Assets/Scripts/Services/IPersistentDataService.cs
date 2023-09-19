using Data;
using Interfaces;

namespace Services
{
    public interface IPersistentDataService : IService
    {
        PlayerProgress Progress { get; }
        GameSettings GameSettings { get; }
        Purchases Purchases { get; }
        Customizations Customizations { get; }

        void InitWithLoadedData(PlayerData loadedPlayerData);
        void RegisterDataReader(IPersistentDataReader reader);
        void ResetProgressAndSave();
        void Save();
    }
}