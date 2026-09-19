using CodeBase.Infrastructure.StateMachineService.StateInfrastructure;
using CodeBase.Infrastructure.StateMachineService.StateMachine;

namespace CodeBase.Gameplay.Board.States
{
    public class InitializeGameState : SimpleState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly GameBoard _gameBoard;

        public InitializeGameState(IGameStateMachine stateMachine, 
            GameBoard gameBoard)
        {
            _stateMachine = stateMachine;
            _gameBoard = gameBoard;
        }

        public override void Enter()
        {
            base.Enter();
            
            _gameBoard.BuildBoard();
            _stateMachine.Enter<IdleState>();
        }
    }
}