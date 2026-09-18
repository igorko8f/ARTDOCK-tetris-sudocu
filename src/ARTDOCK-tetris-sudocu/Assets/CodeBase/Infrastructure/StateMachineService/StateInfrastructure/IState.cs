namespace CodeBase.Infrastructure.StateMachineService.StateInfrastructure
{
    public interface IState : IExitableState
    {
        void Enter();
    }
}