using Data;

namespace Services
{
    public interface ISaveLoadService
    {
        PlayerProgress LoadProgress();
        void SaveProgress();
    }
}