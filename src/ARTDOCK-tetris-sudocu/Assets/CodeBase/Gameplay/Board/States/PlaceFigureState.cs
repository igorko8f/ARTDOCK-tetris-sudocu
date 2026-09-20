using System.Collections.Generic;
using CodeBase.Gameplay.Board.States.Payloads;
using CodeBase.Gameplay.Draggables;
using CodeBase.Gameplay.Figures;
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
                var boardPositions = _gameBoard.GetBoardPositionsAccordingToFigure(figure.GetMatrix(), position);
                if (_gameBoard.CouldPlaceFigureOn(boardPositions))
                {
                    _gameBoard.ActivateCells(boardPositions);
                    _tray.RemoveFigure(figure);
                    
                    _draggableService.ReleaseDraggable();
                    _stateMachine.Enter<CheckLinesState, IEnumerable<(int x, int y)>>(boardPositions);
                    return;
                }
            }
            
            figure.RestorePosition();
            _draggableService.ReleaseDraggable();
            _stateMachine.Enter<IdleState>();
        }
    }
}