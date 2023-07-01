using Constants;
using Data;
using Services;

namespace Infrastructure.StateMachine
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly ISaveLoadService _saveLoadService;

        public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService persistentProgressService,
            ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _persistentProgressService = persistentProgressService;
            _saveLoadService = saveLoadService;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();

            _gameStateMachine.Enter<MainMenuState>();
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
            return new PlayerProgress();
        }
    }
}