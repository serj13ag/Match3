using Data;

namespace Services
{
    public class PersistentProgressService : IPersistentProgressService
    {
        private readonly ISaveService _saveService;

        public PlayerProgress Progress { get; private set; }

        public PersistentProgressService(ISaveService saveService)
        {
            _saveService = saveService;
        }

        public void InitProgress(PlayerProgress playerProgress)
        {
            Progress = playerProgress ?? CreateEmptyPlayerProgress();
        }

        public void SaveProgress()
        {
            _saveService.SaveProgress(Progress);
        }

        public void ResetProgressAndSave()
        {
            Progress = CreateEmptyPlayerProgress();

            _saveService.SaveProgress(Progress);
        }

        private static PlayerProgress CreateEmptyPlayerProgress()
        {
            return new PlayerProgress();
        }
    }
}