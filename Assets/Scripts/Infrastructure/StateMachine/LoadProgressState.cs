using Services;

namespace Infrastructure.StateMachine
{
    public class LoadProgressState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _persistentProgressService;

        public bool IsGameLoopState => false;

        public LoadProgressState(IGameStateMachine gameStateMachine, IPersistentProgressService persistentProgressService)
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