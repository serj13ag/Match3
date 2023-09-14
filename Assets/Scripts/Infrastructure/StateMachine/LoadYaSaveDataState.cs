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
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly ISettingsService _settingsService;
        private readonly ICoinService _coinService;
        private readonly IYaGamesMonoService _yaGamesMonoService;

        public bool IsGameLoopState => false;

        public LoadYaSaveDataState(GameStateMachine gameStateMachine,
            IPersistentProgressService persistentProgressService, ISettingsService settingsService,
            ICoinService coinService, IYaGamesMonoService yaGamesMonoService)
        {
            _gameStateMachine = gameStateMachine;
            _persistentProgressService = persistentProgressService;
            _settingsService = settingsService;
            _coinService = coinService;
            _yaGamesMonoService = yaGamesMonoService;
        }

        public void Enter()
        {
            _yaGamesMonoService.Load(OnPlayerDataLoaded);
        }

        public void Exit()
        {
        }

        private void OnPlayerDataLoaded(string dataString)
        {
            Debug.Log(dataString);

            PlayerProgress progress = null;
            GameSettings settings = null;

            try
            {
                Dictionary<string, string> dataDict = JsonHelper.FromJson<Dictionary<string, string>>(dataString);
                Debug.Log("dict parsed");

                foreach (string dataDictKey in dataDict.Keys)
                {
                    Debug.Log(dataDictKey);
                }

                progress = dataDict.TryGetValue(Settings.ProgressKey, out string progressDataString)
                    ? JsonHelper.FromJson<PlayerProgress>(progressDataString)
                    : null;
                settings = dataDict.TryGetValue(Settings.ProgressKey, out string settingsDataString)
                    ? JsonHelper.FromJson<GameSettings>(settingsDataString)
                    : null;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            _persistentProgressService.InitProgress(progress);
            _settingsService.InitGameSettings(settings);

            _coinService.UpdateCoinsFromProgress();

            _gameStateMachine.Enter<MainMenuState>();
        }
    }
}