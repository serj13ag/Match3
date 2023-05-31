using Enums;
using Services.Mono;

namespace Services
{
    public class GameRoundService
    {
        private readonly SoundMonoService _soundMonoService;
        private readonly UiMonoService _uiMonoService;

        private bool _roundIsActive;

        public bool RoundIsActive => _roundIsActive;

        public GameRoundService(SoundMonoService soundMonoService, UiMonoService uiMonoService)
        {
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
            //AllServices.Instance.GameStateMachine.ReloadLevel(); // TODO: implement
        }
    }
}