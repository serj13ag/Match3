using Data;

namespace Services
{
    public interface IPersistentProgressService : IService
    {
        PlayerProgress Progress { get; }

        void LoadProgressOrInitNew();
        void ResetProgressAndSave();
        void SaveProgress();
    }
}