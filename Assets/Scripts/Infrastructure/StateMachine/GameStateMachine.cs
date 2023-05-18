using System;
using System.Collections.Generic;
using Controllers;

namespace Infrastructure.StateMachine
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;

        private IExitableState _currentState;

        public GameStateMachine(GameData gameData, SceneLoader sceneLoader, LevelLoadingCurtain levelLoadingCurtain,
            ParticleController particleController, SoundController soundController,
            ScreenFaderController screenFaderController, SceneController sceneController)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this, gameData, sceneLoader, particleController,
                    soundController, screenFaderController, sceneController),
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, levelLoadingCurtain),
                [typeof(GameLoopState)] = new GameLoopState(),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _currentState?.Exit();

            TState state = GetState<TState>();
            _currentState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }
    }
}