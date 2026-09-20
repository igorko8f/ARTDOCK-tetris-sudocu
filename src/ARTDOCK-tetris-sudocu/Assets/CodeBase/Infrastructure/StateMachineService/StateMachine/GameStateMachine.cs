using System;
using CodeBase.Infrastructure.StateMachineService.StateInfrastructure;
using RSG;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.StateMachineService.StateMachine
{
    public class GameStateMachine : IGameStateMachine, ITickable, IDisposable
    {
        private readonly IInstantiator _instantiator;
        private IExitableState _activeState;

        public GameStateMachine(DiContainer container)
        {
            _instantiator = container;
            Promise.UnhandledException += LogPromiseException;
        }

        public void Dispose()
        {
            Promise.UnhandledException -= LogPromiseException;
        }

        public void Tick()
        {
            if (_activeState is IUpdateable updateableState)
                updateableState.Update();
        }

        public void Enter<TState>() where TState : class, IState =>
            RequestEnter<TState>()
                .Done();

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload> =>
            RequestEnter<TState, TPayload>(payload)
                .Done();

        private IPromise<TState> RequestEnter<TState>() where TState : class, IState =>
            RequestChangeState<TState>()
                .Then(EnterState);

        private IPromise<TState> RequestEnter<TState, TPayload>(TPayload payload)
            where TState : class, IPayloadState<TPayload> =>
            RequestChangeState<TState>()
                .Then(state => EnterPayloadState(state, payload));

        private TState EnterState<TState>(TState state) where TState : class, IState
        {
            _activeState = state;

            state.Enter();
            return state;
        }

        private TState EnterPayloadState<TState, TPayload>(TState state, TPayload payload)
            where TState : class, IPayloadState<TPayload>
        {
            _activeState = state;

            state.Enter(payload);
            return state;
        }

        private IPromise<TState> RequestChangeState<TState>() where TState : class, IExitableState
        {
            if (_activeState != null)
            {
                return _activeState
                    .BeginExit()
                    .Then(_activeState.EndExit)
                    .Then(ChangeState<TState>);
            }

            return ChangeState<TState>();
        }


        private IPromise<TState> ChangeState<TState>() where TState : class, IExitableState
        {
            TState state = _instantiator.Instantiate<TState>();
            return Promise<TState>.Resolved(state);
        }

        private void LogPromiseException(object sender, ExceptionEventArgs e)
        {
            Debug.LogError($"Error in promise: {sender} - {e.Exception.Message}");
        }
    }
}