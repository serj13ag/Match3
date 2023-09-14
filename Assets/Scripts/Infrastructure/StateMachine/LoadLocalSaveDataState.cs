using Constants;
using Data;
using Helpers;
using Services;
using UnityEngine;

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
            string savedProgressString = PlayerPrefs.GetString(Settings.ProgressKey);
            PlayerProgress progress = string.IsNullOrEmpty(savedProgressString)
                ? null
                : JsonHelper.FromJson<PlayerProgress>(savedProgressString);

            _persistentProgressService.InitProgress(progress);

            string savedSettingsString = PlayerPrefs.GetString(Settings.SettingsKey);
            GameSettings gameSettings = string.IsNullOrEmpty(savedSettingsString)
                ? null
                : JsonHelper.FromJson<GameSettings>(savedSettingsString);

            _settingsService.InitGameSettings(gameSettings);

            _coinService.UpdateCoinsFromProgress();

            _gameStateMachine.Enter<MainMenuState>();
        }

        public void Exit()
        {
        }
    }
}