namespace CodeBase.Infrastructure.StateMachineService.StateInfrastructure
{
    public interface IPayloadState<TPayload> : IExitableState
    {
        void Enter(TPayload payload);
    }
}