using System.Collections.Generic;
using Interfaces;

namespace Services
{
    public class ProgressUpdateService : IProgressUpdateService
    {
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly ISaveLoadService _saveLoadService;

        private readonly List<IProgressWriter> _progressWriters;

        public ProgressUpdateService(IPersistentProgressService persistentProgressService,
            ISaveLoadService saveLoadService)
        {
            _persistentProgressService = persistentProgressService;
            _saveLoadService = saveLoadService;

            _progressWriters = new List<IProgressWriter>();
        }

        public void Register(IProgressWriter progressWriter)
        {
            _progressWriters.Add(progressWriter);
        }

        public void UpdateProgressAndSave()
        {
            foreach (IProgressWriter progressWriter in _progressWriters)
            {
                progressWriter.WriteToProgress(_persistentProgressService.Progress);
            }

            _persistentProgressService.SaveProgress();
        }
    }
}