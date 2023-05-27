using System;
using System.Collections.Generic;

namespace Infrastructure.StateMachine
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;

        private IExitableState _currentState;

        public GameStateMachine(GlobalServices globalServices)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this, globalServices),
                [typeof(LoadProgressState)] = new LoadProgressState(this,
                    globalServices.PersistentProgressService, globalServices.SaveLoadService),
                [typeof(LoadLevelState)] = new LoadLevelState(this, globalServices.SceneLoader,
                    globalServices.LoadingCurtainController, globalServices.AssetProviderService,
                    globalServices.RandomService, globalServices.GameDataService, globalServices.SoundController,
                    globalServices.UpdateController, globalServices.PersistentProgressService),
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