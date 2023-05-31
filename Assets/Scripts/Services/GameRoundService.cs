using Enums;
using Infrastructure.StateMachine;
using Services.Mono;

namespace Services
{
    public class GameRoundService
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SoundMonoService _soundMonoService;
        private readonly UiMonoService _uiMonoService;

        private readonly string _levelName;

        private bool _roundIsActive;

        public bool RoundIsActive => _roundIsActive;

        public GameRoundService(string levelName, GameStateMachine gameStateMachine, SoundMonoService soundMonoService,
            UiMonoService uiMonoService)
        {
            _levelName = levelName;
            _gameStateMachine = gameStateMachine;
            _soundMonoService = soundMonoService;
            _uiMonoService = uiMonoService;
        }

        public void StartGame(int scoreGoal)
        {
            _soundMonoService.PlaySound(SoundType.Music);
            _uiMonoService.ShowStartGameMessageWindow(scoreGoal, StartRound);
        }

        public void EndRound(bool scoreGoalReached)
        {
            if (scoreGoalReached)
            {
                _soundMonoService.PlaySound(SoundType.Win);
                _uiMonoService.ShowGameWinMessageWindow(ReloadLevel);
            }
            else
            {
                _soundMonoService.PlaySound(SoundType.Lose);
                _uiMonoService.ShowGameOverMessageWindow(ReloadLevel);
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