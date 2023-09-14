using Data;

namespace Services
{
    public interface ISaveService : IService
    {
        void SaveProgress(PlayerProgress playerProgress);
        void SaveGameSettings(GameSettings gameSettings);
    }
}