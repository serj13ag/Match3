using Enums;
using Services.Mono;

namespace Services
{
    public class GameRoundService
    {
        private readonly SoundMonoService _soundMonoService;
        private readonly UiMonoService _uiMonoService;

        public GameRoundService(SoundMonoService soundMonoService, UiMonoService uiMonoService)
        {
            _soundMonoService = soundMonoService;
            _uiMonoService = uiMonoService;
        }

        public void StartGame(int scoreGoal)
        {
            _soundMonoService.PlaySound(SoundType.Music);
            _uiMonoService.ShowStartGameMessageWindow(scoreGoal, null);
        }

        public void EndGame(bool scoreGoalReached)
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
        }

        private void ReloadLevel()
        {
            //AllServices.Instance.GameStateMachine.ReloadLevel(); // TODO: implement
        }
    }
}