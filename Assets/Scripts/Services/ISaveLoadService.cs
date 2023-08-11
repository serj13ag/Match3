using Data;

namespace Services
{
    public interface ISaveLoadService : IService
    {
        PlayerProgress LoadProgress();
        void SaveProgress(PlayerProgress playerProgress);

        GameSettings LoadGameSettings();
        void SaveGameSettings(GameSettings gameSettings);
    }
}