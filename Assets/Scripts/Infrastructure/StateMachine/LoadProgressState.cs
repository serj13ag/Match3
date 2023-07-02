using Services;

namespace Infrastructure.StateMachine
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _persistentProgressService;

        public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService persistentProgressService)
        {
            _gameStateMachine = gameStateMachine;
            _persistentProgressService = persistentProgressService;
        }

        public void Enter()
        {
            _persistentProgressService.LoadProgressOrInitNew();

            _gameStateMachine.Enter<MainMenuState>();
        }

        public void Exit()
        {
        }
    }
}