using Infrastructure.StateMachine;
using UnityEngine;

namespace Infrastructure
{
    public class Bootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private GameData _gameData;

        private Game _game;

        private void Awake()
        {
            _game = new Game(_gameData, this);
            _game.GameStateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}