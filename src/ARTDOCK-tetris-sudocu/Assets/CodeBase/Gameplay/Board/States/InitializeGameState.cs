using CodeBase.Gameplay.Figures.Tray;
using CodeBase.Infrastructure.StateMachineService.StateInfrastructure;
using CodeBase.Infrastructure.StateMachineService.StateMachine;

namespace CodeBase.Gameplay.Board.States
{
    public class InitializeGameState : SimpleState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly GameBoard _gameBoard;
        private readonly FigureTray _figureTray;

        public InitializeGameState(IGameStateMachine stateMachine, 
            GameBoard gameBoard,
            FigureTray figureTray)
        {
            _stateMachine = stateMachine;
            _gameBoard = gameBoard;
            _figureTray = figureTray;
        }

        public override void Enter()
        {
            base.Enter();
            
            _gameBoard.BuildBoard();
            _figureTray.BuildTray();

            _stateMachine.Enter<IdleState>();
        }
    }
}