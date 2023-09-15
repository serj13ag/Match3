using System.Collections.Generic;
using Data;
using Interfaces;

namespace Services
{
    public class ProgressUpdateService : IProgressUpdateService
    {
        private readonly string _levelName;
        private readonly IPersistentDataService _persistentDataService;

        private readonly List<IProgressWriter> _progressWriters;

        public ProgressUpdateService(string levelName, IPersistentDataService persistentDataService)
        {
            _levelName = levelName;
            _persistentDataService = persistentDataService;

            _progressWriters = new List<IProgressWriter>();
        }

        public void Register(IProgressWriter progressWriter)
        {
            _progressWriters.Add(progressWriter);
        }

        public void UpdateProgressAndSave()
        {
            if (!_persistentDataService.Progress.BoardData.ContainsKey(_levelName))
            {
                _persistentDataService.Progress.BoardData.Add(_levelName, new LevelBoardData());
            }

            foreach (IProgressWriter progressWriter in _progressWriters)
            {
                progressWriter.WriteToProgress(_persistentDataService.Progress);
            }

            _persistentDataService.Save();
        }
    }
}