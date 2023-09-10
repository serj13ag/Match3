using Services;

namespace Infrastructure.StateMachine
{
    public class LoadLocalSaveDataState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly ISettingsService _settingsService;
        private readonly ICoinService _coinService;
        public bool IsGameLoopState => false;

        public LoadLocalSaveDataState(IGameStateMachine gameStateMachine,
            IPersistentProgressService persistentProgressService, ISettingsService settingsService,
            ICoinService coinService)
        {
            _gameStateMachine = gameStateMachine;
            _persistentProgressService = persistentProgressService;
            _settingsService = settingsService;
            _coinService = coinService;
        }

        public void Enter()
        {
            _persistentProgressService.LoadProgressOrInitNew();
            _settingsService.LoadGameSettings();
            _coinService.UpdateCoinsFromProgress();

            _gameStateMachine.Enter<MainMenuState>();
        }

        public void Exit()
        {
        }
    }
}