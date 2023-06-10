using Enums;
using Infrastructure.StateMachine;
using Services.Mono.Sound;
using Services.UI;

namespace Services
{
    public class GameRoundService
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly ISoundMonoService _soundMonoService;
        private readonly IWindowService _windowService;

        private readonly string _levelName;

        private bool _roundIsActive;

        public bool RoundIsActive => _roundIsActive;

        public GameRoundService(string levelName, GameStateMachine gameStateMachine, ISoundMonoService soundMonoService,
            IWindowService windowService)
        {
            _levelName = levelName;
            _gameStateMachine = gameStateMachine;
            _soundMonoService = soundMonoService;
            _windowService = windowService;
        }

        public void StartGame(int scoreGoal)
        {
            _soundMonoService.PlayBackgroundMusic();
            _windowService.ShowStartGameMessageWindow(scoreGoal, StartRound);
        }

        public void EndRound(bool scoreGoalReached)
        {
            if (scoreGoalReached)
            {
                _soundMonoService.PlaySound(SoundType.Win);
                _windowService.ShowGameWinMessageWindow(ReloadLevel);
            }
            else
            {
                _soundMonoService.PlaySound(SoundType.Lose);
                _windowService.ShowGameOverMessageWindow(ReloadLevel);
            }

            _roundIsActive = false;
        }

        private void StartRound()
        {
            _roundIsActive = true;
        }

        private void ReloadLevel()
        {
            _gameStateMachine.Enter<GameLoopState, string>(_levelName);
        }
    }
}