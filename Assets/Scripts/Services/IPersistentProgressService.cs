using Data;

namespace Services
{
    public interface IPersistentProgressService : IService
    {
        PlayerProgress Progress { get; }

        void InitProgress(PlayerProgress playerProgress);
        void ResetProgressAndSave();
        void SaveProgress();
    }
}