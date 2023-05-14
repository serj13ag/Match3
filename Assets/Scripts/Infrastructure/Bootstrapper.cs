using UnityEngine;

namespace Infrastructure
{
    public class Bootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private LevelLoadingCurtain _levelLoadingCurtain;

        private Game _game;

        private void Awake()
        {
            _game = new Game(this, _levelLoadingCurtain);
            _game.GameStateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}