using Infrastructure.StateMachine;
using Services;
using UnityEngine;

namespace UI.Background
{
    public class EndlessBackgroundScreen : BaseBackgroundScreen
    {
        [SerializeField] private ScoreCounter _scoreCounter;

        public void Init(GameStateMachine gameStateMachine, IScoreService scoreService, ICameraService cameraService)
        {
            InitInner(gameStateMachine, cameraService);

            _scoreCounter.Init(scoreService);
        }
    }
}