using System;
using Enums;
using EventArguments;
using Services.Mono.Sound;
using Services.UI;

namespace Services.GameRound
{
    public class EndlessGameRoundService : IGameRoundService
    {
        private readonly ISoundMonoService _soundMonoService;
        private readonly IWindowService _windowService;
        private readonly ICoinService _coinService;
        private readonly IScoreService _scoreService;
        private readonly IPlayerLevelService _playerLevelService;

        private bool _roundIsActive;

        public bool RoundIsActive => _roundIsActive;

        public EndlessGameRoundService(ISoundMonoService soundMonoService, IWindowService windowService,
            ICoinService coinService, IScoreService scoreService, IPlayerLevelService playerLevelService)
        {
            _soundMonoService = soundMonoService;
            _windowService = windowService;
            _coinService = coinService;
            _scoreService = scoreService;
            _playerLevelService = playerLevelService;

            scoreService.OnScoreChanged += OnScoreChanged;
        }

        public void StartGame()
        {
            _windowService.ShowStartGameMessageWindow(_scoreService.ScoreGoal, StartRound);
        }

        public void EndRound()
        {
            throw new NotImplementedException();
        }

        private void OnScoreChanged(object sender, ScoreChangedEventArgs e)
        {
            if (_scoreService.ScoreGoalReached)
            {
                _soundMonoService.PlaySound(SoundType.Win);
                _windowService.ShowGameWinMessageWindow(UpdatePlayerLevel); // TODO new level window

                _roundIsActive = false;
            }
        }

        private void UpdatePlayerLevel()
        {
            _playerLevelService.GoToNextLevel();
            _coinService.IncrementCoins();
            _scoreService.SetScoreGoal(_playerLevelService.ScoreToNextLevel);

            StartRound();
        }

        private void StartRound()
        {
            _roundIsActive = true;
        }
    }
}