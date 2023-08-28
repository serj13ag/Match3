using Data;

namespace Services
{
    public interface IPersistentProgressService : IService
    {
        PlayerProgress Progress { get; }

        void ResetProgressAndSave();
        void SaveProgress();
    }
}