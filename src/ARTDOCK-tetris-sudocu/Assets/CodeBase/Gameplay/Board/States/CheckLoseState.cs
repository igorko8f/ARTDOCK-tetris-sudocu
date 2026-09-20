using CodeBase.Gameplay.Common.Extensions;
using CodeBase.Gameplay.Figures;
using CodeBase.Gameplay.Figures.Tray;
using CodeBase.Infrastructure.StateMachineService.StateInfrastructure;
using CodeBase.Infrastructure.StateMachineService.StateMachine;

namespace CodeBase.Gameplay.Board.States
{
    public class CheckLoseState : SimpleState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly GameBoard _board;
        private readonly FigureTray _tray;

        public CheckLoseState(IGameStateMachine stateMachine,
            GameBoard board,
            FigureTray tray)
        {
            _stateMachine = stateMachine;
            _board = board;
            _tray = tray;
        }
        
        public override void Enter()
        {
            base.Enter();

            var remainedFigures = _tray.GetRemainedFigures();

            foreach (var remainedFigure in remainedFigures)
            {
                if (CouldPlaceFigure(remainedFigure))
                {
                    _stateMachine.Enter<IdleState>();
                    return;
                }
            }
            
            _stateMachine.Enter<LoseState>();
        }

        private bool CouldPlaceFigure(Figure remainedFigure)
        {
            var figureMatrix = remainedFigure.GetMatrix();
            for (int i = 0; i < 4; i++)
            {
                if (_board.CouldPlaceFigure(figureMatrix))
                    return true;

                figureMatrix = figureMatrix.Rotate(true);
            }

            return false;
        }
    }
}