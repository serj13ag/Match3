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

        public bool IsGameLoopState => false;

        public LoadLocalSaveDataState(IGameStateMachine gameStateMachine, IPersistentDataService persistentDataService)
        {
            _gameStateMachine = gameStateMachine;
            _persistentDataService = persistentDataService;
        }

        public void Enter()
        {
            string savedDataString = PlayerPrefs.GetString(Settings.SavedPlayerDataKey);
            PlayerData savedPlayerData = string.IsNullOrEmpty(savedDataString)
                ? null
                : JsonHelper.FromJson<PlayerData>(savedDataString);

            _persistentDataService.InitWithLoadedData(savedPlayerData);

            _gameStateMachine.Enter<MainMenuState>();
        }

        public void Exit()
        {
        }
    }
}