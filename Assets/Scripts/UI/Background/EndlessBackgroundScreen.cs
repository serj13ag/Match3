using System;
using EventArguments;
using Infrastructure.StateMachine;
using Services;
using TMPro;
using UnityEngine;

namespace UI.Background
{
    public class EndlessBackgroundScreen : BaseBackgroundScreen
    {
        [SerializeField] private TMP_Text _playerLevel;
        [SerializeField] private ScoreCounter _scoreCounter;

        public void Init(GameStateMachine gameStateMachine, IPlayerLevelService playerLevelService,
            IScoreService scoreService, ICameraService cameraService)
        {
            InitInner(gameStateMachine, cameraService);

            _scoreCounter.Init(scoreService);

            UpdatePlayerLevelText(playerLevelService.CurrentLevel);
            playerLevelService.OnCurrentLevelChanged += UpdatePlayerLevelText;
        }

        private void UpdatePlayerLevelText(object sender, PlayerLevelChangedEventArgs e)
        {
            UpdatePlayerLevelText(e.Level);
        }

        private void UpdatePlayerLevelText(int currentLevel)
        {
            _playerLevel.text = currentLevel.ToString();
        }
    }
}