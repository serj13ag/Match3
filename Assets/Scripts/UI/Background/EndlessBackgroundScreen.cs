using EventArguments;
using Services;
using Services.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Background
{
    public class EndlessBackgroundScreen : BaseBackgroundScreen
    {
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private TMP_Text _playerLevel;
        [SerializeField] private ScoreCounter _scoreCounter;

        public void Init(IPlayerLevelService playerLevelService, IScoreService scoreService,
            ICameraService cameraService, IWindowService windowService, ICustomizationService customizationService)
        {
            InitInner(cameraService, windowService);

            _scoreCounter.Init(scoreService);
            _backgroundImage.color = customizationService.GetCurrentBackgroundColor();
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