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
                [typeof(MainMenuState)] = new MainMenuState(this, globalServices.SceneLoader, globalServices.UiFactory,
                    globalServices.StaticDataService),
                [typeof(GameLoopState)] = new GameLoopState(this, globalServices.SceneLoader,
                    globalServices.LoadingCurtainMonoService, globalServices.AssetProviderService,
                    globalServices.RandomService, globalServices.StaticDataService, globalServices.SoundMonoService,
                    globalServices.UpdateMonoService, globalServices.PersistentProgressService,
                    globalServices.UiFactory, globalServices.WindowService, globalServices.SaveLoadService),
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