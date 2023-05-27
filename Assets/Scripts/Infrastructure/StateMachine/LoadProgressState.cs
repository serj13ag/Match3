using Data;
using Services;
using Services.PersistentProgress;

namespace Infrastructure.StateMachine
{
    public class LoadProgressState : IState
    {
        private const string Level1SceneName = "Level_1";

        private readonly GameStateMachine _gameStateMachine;
        private readonly PersistentProgressService _persistentProgressService;
        private readonly SaveLoadService _saveLoadService;

        public LoadProgressState(GameStateMachine gameStateMachine, PersistentProgressService persistentProgressService,
            SaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _persistentProgressService = persistentProgressService;
            _saveLoadService = saveLoadService;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();

            _gameStateMachine.Enter<LoadLevelState, string>(_persistentProgressService.Progress.BoardData.LevelBoardData.LevelName);
        }

        public void Exit()
        {
        }

        private void LoadProgressOrInitNew()
        {
            _persistentProgressService.Progress = _saveLoadService.LoadProgress() ?? CreatePlayerProgress();
        }

        private static PlayerProgress CreatePlayerProgress()
        {
            return new PlayerProgress(Level1SceneName);
        }
    }
}