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
        private readonly ISettingsService _settingsService;
        private readonly ICoinService _coinService;
        private readonly IYaGamesMonoService _yaGamesMonoService;

        public bool IsGameLoopState => false;

        public LoadYaSaveDataState(GameStateMachine gameStateMachine,
            IPersistentDataService persistentDataService, ISettingsService settingsService,
            ICoinService coinService, IYaGamesMonoService yaGamesMonoService)
        {
            _gameStateMachine = gameStateMachine;
            _persistentDataService = persistentDataService;
            _settingsService = settingsService;
            _coinService = coinService;
            _yaGamesMonoService = yaGamesMonoService;
        }

        public void Enter()
        {
            _yaGamesMonoService.Load(Settings.SavedPlayerDataKey, OnPlayerDataLoaded);
        }

        public void Exit()
        {
        }

        private void OnPlayerDataLoaded(string dataString)
        {
            PlayerData playerData = GetPlayerData(dataString);

            _persistentDataService.InitData(playerData);
            _settingsService.InitGameSettings();
            _coinService.UpdateCoinsFromProgress();

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
                playerData = string.IsNullOrEmpty(dataString)
                    ? null
                    : JsonHelper.FromJson<PlayerData>(dataString);
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