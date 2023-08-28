using Data;

namespace Services
{
    public class PersistentProgressService : IPersistentProgressService
    {
        private readonly ISaveLoadService _saveLoadService;

        public PlayerProgress Progress { get; private set; }

        public PersistentProgressService(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;

            LoadProgressOrInitNew();
        }

        public void SaveProgress()
        {
            _saveLoadService.SaveProgress(Progress);
        }

        public void ResetProgressAndSave()
        {
            Progress = CreateEmptyPlayerProgress();

            _saveLoadService.SaveProgress(Progress);
        }

        private void LoadProgressOrInitNew()
        {
            Progress = _saveLoadService.LoadProgress() ?? CreateEmptyPlayerProgress();
        }

        private static PlayerProgress CreateEmptyPlayerProgress()
        {
            return new PlayerProgress();
        }
    }
}