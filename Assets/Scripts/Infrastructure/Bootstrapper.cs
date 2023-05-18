using Controllers;
using Infrastructure.StateMachine;
using UnityEngine;

namespace Infrastructure
{
    public class Bootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private LevelLoadingCurtain _levelLoadingCurtain;
        
        [SerializeField] private ParticleController _particleController;
        [SerializeField] private SoundController _soundController;
        [SerializeField] private ScreenFaderController _screenFaderController;
        [SerializeField] private SceneController _sceneController;

        [SerializeField] private GameData _gameData;

        private Game _game;

        private void Awake()
        {
            _game = new Game(_gameData,
                this,
                _levelLoadingCurtain,
                _particleController, _soundController, _screenFaderController, _sceneController);
            _game.GameStateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}