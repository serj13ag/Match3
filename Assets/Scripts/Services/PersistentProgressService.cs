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
        }

        public void LoadProgressOrInitNew()
        {
            Progress = _saveLoadService.LoadProgress() ?? CreateEmptyPlayerProgress();
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

        private static PlayerProgress CreateEmptyPlayerProgress()
        {
            return new PlayerProgress();
        }
    }
}