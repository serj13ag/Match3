using Data;

namespace Services
{
    public interface IPersistentProgressService
    {
        PlayerProgress Progress { get; set; }
        void ResetProgress();
    }
}