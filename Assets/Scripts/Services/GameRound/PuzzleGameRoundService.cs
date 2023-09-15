using Enums;
using EventArguments;
using Infrastructure.StateMachine;
using Services.Mono.Sound;
using Services.UI;

namespace Services.GameRound
{
    public class PuzzleGameRoundService : IGameRoundService
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISoundMonoService _soundMonoService;
        private readonly IWindowService _windowService;
        private readonly IPersistentDataService _persistentDataService;
        private readonly IScoreService _scoreService;

        private readonly string _levelName;

        private bool _roundIsActive;

        public bool RoundIsActive => _roundIsActive;

        public PuzzleGameRoundService(string levelName, IGameStateMachine gameStateMachine, ISoundMonoService soundMonoService,
            IWindowService windowService, IPersistentDataService persistentDataService, IScoreService scoreService)
        {
            _levelName = levelName;
            _gameStateMachine = gameStateMachine;
            _soundMonoService = soundMonoService;
            _windowService = windowService;
            _persistentDataService = persistentDataService;
            _scoreService = scoreService;

            scoreService.OnScoreChanged += OnScoreChanged;
        }

        public void StartGame()
        {
            _windowService.ShowStartGameMessageWindow(_scoreService.ScoreGoal, StartRound);
        }

        public void EndRound()
        {
            if (_scoreService.ScoreGoalReached)
            {
                _soundMonoService.PlaySound(SoundType.Win);
                _windowService.ShowGameWinMessageWindow(ReloadLevel);
            }
            else
            {
                _soundMonoService.PlaySound(SoundType.Lose);
                _windowService.ShowGameOverMessageWindow(ReloadLevel);
            }

            _persistentDataService.ResetProgressAndSave();

            _roundIsActive = false;
        }

        private void OnScoreChanged(object sender, ScoreChangedEventArgs e)
        {
            if (_scoreService.ScoreGoalReached)
            {
                EndRound();
            }
        }

        private void StartRound()
        {
            _roundIsActive = true;
        }

        private void ReloadLevel()
        {
            _gameStateMachine.Enter<PuzzleGameLoopState, string>(_levelName);
        }
    }
}