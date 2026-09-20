using CodeBase.Infrastructure.Input;
using CodeBase.Infrastructure.StateMachineService.StateInfrastructure;
using CodeBase.Infrastructure.StateMachineService.StateMachine;

namespace CodeBase.Gameplay.Board.States
{
    public class IdleState : SimpleState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IInputService _inputService;

        public IdleState(IGameStateMachine stateMachine,
            IInputService inputService)
        {
            _stateMachine = stateMachine;
            _inputService = inputService;
        }

        public override void Enter()
        {
            base.Enter();
            
            _inputService.EnableInput();
        }
    }
}