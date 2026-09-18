using CodeBase.Infrastructure.StateMachineService.StateInfrastructure;

namespace CodeBase.Infrastructure.StateMachineService.StateMachine
{
    public interface IGameStateMachine
    {
        void Enter<TState>() where TState : class, IState;
        void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>;
    }
}