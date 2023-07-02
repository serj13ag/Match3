using Data;

namespace Services
{
    public interface IPersistentProgressService
    {
        PlayerProgress Progress { get; }

        void LoadProgressOrInitNew();
        void ResetProgressAndSave();
        void SaveProgress();
    }
}