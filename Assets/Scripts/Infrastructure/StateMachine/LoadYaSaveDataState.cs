using System;
using Constants;
using Data;
using Helpers;
using Interfaces;
using Services;
using Services.Mono;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class LoadYaSaveDataState : IState, IUpdatable
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentDataService _persistentDataService;
        private readonly IYaGamesMonoService _yaGamesMonoService;

        private float _timeTillRequestLoadData;
        private bool _loadDataRequestInvoked;

        public bool IsGameLoopState => false;

        public LoadYaSaveDataState(GameStateMachine gameStateMachine, IPersistentDataService persistentDataService,
            IYaGamesMonoService yaGamesMonoService)
        {
            _gameStateMachine = gameStateMachine;
            _persistentDataService = persistentDataService;
            _yaGamesMonoService = yaGamesMonoService;

            _timeTillRequestLoadData = Settings.TimeTillRequestLoadData;
        }

        public void Enter()
        {
            Debug.Log($"Entered {nameof(LoadYaSaveDataState)}");
        }

        public void Exit()
        {
        }

        public void OnUpdate(float deltaTime)
        {
            if (_loadDataRequestInvoked)
            {
                return;
            }

            if (_timeTillRequestLoadData < 0)
            {
                _yaGamesMonoService.Load(Settings.SavedPlayerDataKey, OnPlayerDataLoaded);
                _loadDataRequestInvoked = true;
            }
            else
            {
                _timeTillRequestLoadData -= deltaTime;
            }
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