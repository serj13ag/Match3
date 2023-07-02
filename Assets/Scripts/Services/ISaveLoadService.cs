using Data;

namespace Services
{
    public interface ISaveLoadService
    {
        PlayerProgress LoadProgress();
        void SaveProgress(PlayerProgress playerProgress);

        GameSettings LoadGameSettings();
        void SaveGameSettings(GameSettings gameSettings);
    }
}