using System.Collections.Generic;
using Data;
using Interfaces;

namespace Services
{
    public class ProgressUpdateService : IProgressUpdateService
    {
        private readonly string _levelName;
        private readonly IPersistentProgressService _persistentProgressService;

        private readonly List<IProgressWriter> _progressWriters;

        public ProgressUpdateService(string levelName, IPersistentProgressService persistentProgressService)
        {
            _levelName = levelName;
            _persistentProgressService = persistentProgressService;

            _progressWriters = new List<IProgressWriter>();
        }

        public void Register(IProgressWriter progressWriter)
        {
            _progressWriters.Add(progressWriter);
        }

        public void UpdateProgressAndSave()
        {
            if (!_persistentProgressService.Progress.BoardData.ContainsKey(_levelName))
            {
                _persistentProgressService.Progress.BoardData.Add(_levelName, new LevelBoardData());
            }

            foreach (IProgressWriter progressWriter in _progressWriters)
            {
                progressWriter.WriteToProgress(_persistentProgressService.Progress);
            }

            _persistentProgressService.SaveProgress();
        }
    }
}