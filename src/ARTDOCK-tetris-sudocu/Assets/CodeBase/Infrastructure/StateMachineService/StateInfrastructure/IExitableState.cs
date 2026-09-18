using RSG;

namespace CodeBase.Infrastructure.StateMachineService.StateInfrastructure
{
    public interface IExitableState
    {
        IPromise BeginExit();
        void EndExit();
    }
}