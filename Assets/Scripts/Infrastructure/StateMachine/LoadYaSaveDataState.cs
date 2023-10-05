using System;
using Constants;
using Data;
using Helpers;
using Services;
using Services.Mono;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class LoadYaSaveDataState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentDataService _persistentDataService;
        private readonly IYaGamesMonoService _yaGamesMonoService;

        private bool _loadDataRequestInvoked;

        public bool IsGameLoopState => false;

        public LoadYaSaveDataState(GameStateMachine gameStateMachine, IPersistentDataService persistentDataService,
            IYaGamesMonoService yaGamesMonoService)
        {
            _gameStateMachine = gameStateMachine;
            _persistentDataService = persistentDataService;
            _yaGamesMonoService = yaGamesMonoService;
        }

        public void Enter()
        {
            Debug.Log($"Entered {nameof(LoadYaSaveDataState)}");

            _yaGamesMonoService.InitYaSDK(OnSDKInitCompleted);
        }

        public void Exit()
        {
            Debug.Log($"Exited {nameof(LoadYaSaveDataState)}");
        }

        private void OnSDKInitCompleted()
        {
            _yaGamesMonoService.Load(Settings.SavedPlayerDataKey, OnPlayerDataLoaded);
        }

        private void OnPlayerDataLoaded(string dataString)
        {
            PlayerData playerData = string.IsNullOrEmpty(dataString)
                ? null
                : GetPlayerData(dataString);

            _persistentDataService.InitWithLoadedData(playerData);

            _gameStateMachine.Enter<MainMenuState>();
        }

        private static PlayerData GetPlayerData(string dataString)
        {
            Debug.Log($"Raw player data string: {dataString}");

            dataString = PrepareDataStringForDeserializing(dataString);

            Debug.Log($"Prepared player data string: {dataString}");

            PlayerData playerData = null;

            try
            {
                playerData = JsonHelper.FromJson<PlayerData>(dataString);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error when deserializing player data: {e}");
                Console.WriteLine(e);
            }

            return playerData;
        }

        private static string PrepareDataStringForDeserializing(string dataString)
        {
            dataString = dataString.Remove(0, 2);
            dataString = dataString.Remove(dataString.Length - 2, 2);
            dataString = dataString.Replace(@"\\\", '\u0002'.ToString());
            dataString = dataString.Replace(@"\", "");
            dataString = dataString.Replace('\u0002'.ToString(), @"\");

            return dataString;
        }
    }
}