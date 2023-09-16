using System;
using System.Collections.Generic;
using Interfaces;
using Services;
using Services.Mono;
using Services.Mono.Sound;
using Services.UI;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;

        private IExitableState _currentState;

        public bool InGameLoopState => _currentState.IsGameLoopState;

        public GameStateMachine(ServiceLocator serviceLocator)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(LoadLocalSaveDataState)] = new LoadLocalSaveDataState(
                    this,
                    serviceLocator.Get<IPersistentDataService>(),
                    serviceLocator.Get<ISettingsService>(),
                    serviceLocator.Get<ICoinService>()),
                [typeof(MainMenuState)] = new MainMenuState(
                    this,
                    serviceLocator.Get<ISceneLoader>(),
                    serviceLocator.Get<IUiFactory>(),
                    serviceLocator.Get<ISoundMonoService>(),
                    serviceLocator.Get<ILoadingCurtainMonoService>(),
                    serviceLocator.Get<IWindowService>()),
                [typeof(EndlessGameLoopState)] = new EndlessGameLoopState(
                    serviceLocator.Get<ISceneLoader>(),
                    serviceLocator.Get<ILoadingCurtainMonoService>(),
                    serviceLocator.Get<IAssetProviderService>(),
                    serviceLocator.Get<IRandomService>(),
                    serviceLocator.Get<IStaticDataService>(),
                    serviceLocator.Get<ISoundMonoService>(),
                    serviceLocator.Get<IUpdateMonoService>(),
                    serviceLocator.Get<IPersistentDataService>(),
                    serviceLocator.Get<IUiFactory>(),
                    serviceLocator.Get<IWindowService>(),
                    serviceLocator.Get<ICoinService>()),
                [typeof(PuzzleGameLoopState)] = new PuzzleGameLoopState(
                    this,
                    serviceLocator.Get<ISceneLoader>(),
                    serviceLocator.Get<ILoadingCurtainMonoService>(),
                    serviceLocator.Get<IAssetProviderService>(),
                    serviceLocator.Get<IRandomService>(),
                    serviceLocator.Get<IStaticDataService>(),
                    serviceLocator.Get<ISoundMonoService>(),
                    serviceLocator.Get<IUpdateMonoService>(),
                    serviceLocator.Get<IPersistentDataService>(),
                    serviceLocator.Get<IUiFactory>(),
                    serviceLocator.Get<IWindowService>()),
            };

            if (!Application.isEditor)
            {
                _states.Add(typeof(LoadYaSaveDataState), new LoadYaSaveDataState(
                    this,
                    serviceLocator.Get<IPersistentDataService>(),
                    serviceLocator.Get<ISettingsService>(),
                    serviceLocator.Get<ICoinService>(),
                    serviceLocator.Get<IYaGamesMonoService>()));
            }
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

        public void Update(float deltaTime)
        {
            if (_currentState is IUpdatable updatableState)
            {
                updatableState.OnUpdate(deltaTime);
            }
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