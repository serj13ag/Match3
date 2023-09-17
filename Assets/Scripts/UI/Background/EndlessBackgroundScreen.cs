using EventArguments;
using Services;
using Services.UI;
using TMPro;
using UnityEngine;

namespace UI.Background
{
    public class EndlessBackgroundScreen : BaseBackgroundScreen
    {
        [SerializeField] private TMP_Text _playerLevel;
        [SerializeField] private ScoreCounter _scoreCounter;

        public void Init(IPlayerLevelService playerLevelService, IScoreService scoreService,
            ICameraService cameraService, IWindowService windowService)
        {
            InitInner(cameraService, windowService);

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