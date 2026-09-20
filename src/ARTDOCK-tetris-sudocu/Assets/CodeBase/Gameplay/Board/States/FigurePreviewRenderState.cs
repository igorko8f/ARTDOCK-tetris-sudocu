using CodeBase.Gameplay.Draggables;
using CodeBase.Gameplay.Figures;
using CodeBase.Infrastructure.Input;
using CodeBase.Infrastructure.StateMachineService.StateInfrastructure;
using R3;

namespace CodeBase.Gameplay.Board.States
{
    public class FigurePreviewRenderState : SimpleState, IUpdateable
    {
        private readonly IDraggableService _draggableService;
        private readonly GameBoard _gameBoard;
        private readonly IInputService _inputService;
        private readonly CompositeDisposable _compositeDisposable;
        
        private (int x, int y) _prevCellIndex = (-1, -1);
        private Figure _figure;

        public FigurePreviewRenderState(IDraggableService draggableService,
            GameBoard gameBoard,
            IInputService inputService)
        {
            _draggableService = draggableService;
            _gameBoard = gameBoard;
            _inputService = inputService;
            _compositeDisposable = new CompositeDisposable();
        }
        
        public override void Enter()
        {
            base.Enter();
            
            _inputService.RotatePressed
                .Subscribe(_ => OnFigureRotated())
                .AddTo(_compositeDisposable);
            
            _figure = _draggableService.CurrentDraggableFigure;
        }

        protected override void Exit()
        {
            _compositeDisposable.Dispose();
            base.Exit();
        }

        public void Update()
        {
            var figurePosition = _figure.GetPosition();
            if (!_gameBoard.TryGetCellAt(figurePosition, out var position))
            {
                _gameBoard.ResetPreviewState();
                return;
            }
            
            if (position == _prevCellIndex)
                return;

            _gameBoard.ResetPreviewState();
            if (_gameBoard.CouldPlaceFigureOn(_figure.GetMatrix(), position))
                _gameBoard.ActivateCells(_figure.GetMatrix(), position, true);
            
            _prevCellIndex = position;
        }

        private void OnFigureRotated()
        {
            _prevCellIndex = (-1, -1);
        }
    }
}