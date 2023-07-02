using Data;

namespace Services
{
    public class PersistentProgressService : IPersistentProgressService
    {
        public PlayerProgress Progress { get; set; }

        public void ResetProgress()
        {
            Progress = new PlayerProgress();
        }
    }
}