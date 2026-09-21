using CodeBase.Infrastructure.SaveLoad;
using CodeBase.Infrastructure.StateMachineService.StateInfrastructure;
using CodeBase.Infrastructure.StateMachineService.StateMachine;

namespace CodeBase.Infrastructure.StateMachineService.States
{
    public class BootstrapState : SimpleState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly ISaveService _saveService;

        public BootstrapState(IGameStateMachine stateMachine, ISaveService saveService)
        {
            _stateMachine = stateMachine;
            _saveService = saveService;
        }

        public override void Enter()
        {
            base.Enter();

            _saveService.Load();
            
            _stateMachine.Enter<EnterGameplaySceneState>();
        }
    }
}