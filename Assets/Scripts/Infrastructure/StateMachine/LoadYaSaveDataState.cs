using System;
using System.Collections.Generic;
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
            Debug.Log(dataString);

            PlayerData playerData = null;

            try
            {
                dataString = dataString.Remove(0, 2);
                dataString = dataString.Remove(dataString.Length - 2, 2);
                dataString = dataString.Replace(@"\\\", '\u0002'.ToString());
                dataString = dataString.Replace(@"\", "");
                dataString = dataString.Replace('\u0002'.ToString(), @"\");
                
                Debug.Log(dataString);
                
                playerData = string.IsNullOrEmpty(dataString)
                    ? null
                    : JsonHelper.FromJson<PlayerData>(dataString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            _persistentDataService.InitData(playerData);
            _settingsService.InitGameSettings();
            _coinService.UpdateCoinsFromProgress();

            _gameStateMachine.Enter<MainMenuState>();
        }
    }
}