using CodeBase.Gameplay.Draggables;
using CodeBase.Gameplay.Figures.Tray;
using CodeBase.Infrastructure.Input;
using CodeBase.Infrastructure.StateMachineService.StateInfrastructure;
using CodeBase.Infrastructure.StateMachineService.StateMachine;

namespace CodeBase.Gameplay.Board.States
{
    public class PlaceFigureState : SimpleState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IInputService _inputService;
        private readonly IDraggableService _draggableService;
        private readonly GameBoard _gameBoard;
        private readonly FigureTray _tray;

        public PlaceFigureState(IGameStateMachine stateMachine,
            IInputService inputService,
            IDraggableService draggableService,
            GameBoard gameBoard,
            FigureTray tray)
        {
            _stateMachine = stateMachine;
            _inputService = inputService;
            _draggableService = draggableService;
            _gameBoard = gameBoard;
            _tray = tray;
        }

        public override void Enter()
        {
            base.Enter();

            var figure = _draggableService.CurrentDraggableFigure;

            _inputService.DisableInput();
            
            var figurePosition = figure.GetPosition();
            if (_gameBoard.TryGetCellAt(figurePosition, out var position))
            {
                if (_gameBoard.CouldPlaceFigureOn(figure.GetMatrix(), position))
                {
                    _gameBoard.ActivateCells(figure.GetMatrix(), position);
                    _tray.RemoveFigure(figure);
                    
                    _draggableService.ReleaseDraggable();
                    _stateMachine.Enter<IdleState>();
                    return;
                }
            }
            
            figure.RestorePosition();
            _draggableService.ReleaseDraggable();
            _stateMachine.Enter<IdleState>();
        }
    }
}