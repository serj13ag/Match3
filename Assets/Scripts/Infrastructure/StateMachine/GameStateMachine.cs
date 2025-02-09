﻿using System;
using System.Collections.Generic;
using Services;
using Services.Mono;
using Services.Mono.Sound;
using Services.UI;

namespace Infrastructure.StateMachine
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;

        private IExitableState _currentState;

        public bool InGameLoopState => _currentState.IsGameLoopState;

        public GameStateMachine(ServiceLocator serviceLocator, bool isYaGamesEnvironment)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(MainMenuState)] = new MainMenuState(
                    this,
                    serviceLocator.Get<ISceneLoader>(),
                    serviceLocator.Get<IUiFactory>(),
                    serviceLocator.Get<ISoundMonoService>(),
                    serviceLocator.Get<ILoadingCurtainMonoService>(),
                    serviceLocator.Get<IWindowService>(),
                    serviceLocator.Get<IYaGamesMonoService>()),
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
                    serviceLocator.Get<ICoinService>(),
                    serviceLocator.Get<IAdsService>(),
                    serviceLocator.Get<ICustomizationService>()),
            };

            if (isYaGamesEnvironment)
            {
                _states.Add(typeof(LoadYaSaveDataState), new LoadYaSaveDataState(
                    this,
                    serviceLocator.Get<IPersistentDataService>(),
                    serviceLocator.Get<IYaGamesMonoService>()));
            }
            else
            {
                _states.Add(typeof(LoadLocalSaveDataState), new LoadLocalSaveDataState(
                    this,
                    serviceLocator.Get<IPersistentDataService>()));
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