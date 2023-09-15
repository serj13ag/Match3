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
        private readonly IPersistentDataService _persistentDataService;
        private readonly ISettingsService _settingsService;
        private readonly ICoinService _coinService;
        public bool IsGameLoopState => false;

        public LoadLocalSaveDataState(IGameStateMachine gameStateMachine,
            IPersistentDataService persistentDataService, ISettingsService settingsService,
            ICoinService coinService)
        {
            _gameStateMachine = gameStateMachine;
            _persistentDataService = persistentDataService;
            _settingsService = settingsService;
            _coinService = coinService;
        }

        public void Enter()
        {
            string savedDataString = PlayerPrefs.GetString(Settings.SavedPlayerDataKey);
            PlayerData savedPlayerData = string.IsNullOrEmpty(savedDataString)
                ? null
                : JsonHelper.FromJson<PlayerData>(savedDataString);

            _persistentDataService.InitData(savedPlayerData);
            _settingsService.InitGameSettings();
            _coinService.UpdateCoinsFromProgress();

            _gameStateMachine.Enter<MainMenuState>();
        }

        public void Exit()
        {
        }
    }
}