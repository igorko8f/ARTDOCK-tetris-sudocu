using System;
using CodeBase.Gameplay.Board.Factory;
using CodeBase.Gameplay.Board.States;
using CodeBase.Gameplay.Cells;
using CodeBase.Gameplay.Cells.Factory;
using CodeBase.Infrastructure.ResourcesProvider;
using CodeBase.Infrastructure.StateMachineService.StateMachine;

namespace CodeBase.Gameplay.Board
{
    public class GameBoard : IDisposable
    {
        private readonly IProjectResourcesProvider _resourcesProvider;
        private readonly IBoardFactory _boardFactory;
        private readonly IBoardCellsFactory _cellsFactory;

        private readonly GameBoardConfiguration _configuration;

        private GameBoardView _view;
        private BoardCell[,] _boardCells;
        
        public GameBoard(IProjectResourcesProvider resourcesProvider,
            IBoardFactory boardFactory,
            IBoardCellsFactory cellsFactory)
        {
            _resourcesProvider = resourcesProvider;
            _boardFactory = boardFactory;
            _cellsFactory = cellsFactory;

            _configuration = resourcesProvider.LoadResource<GameBoardConfiguration>();
        }

        public void BuildBoard()
        {
            CreateBoardView();
            FillBoardWithCells();
        }

        private void CreateBoardView() => 
            _view = _boardFactory.CreateBoardView(_configuration.Board_With, _configuration.Board_Height);

        private void FillBoardWithCells()
        {
            _boardCells = new BoardCell[_configuration.Board_With, _configuration.Board_Height];

            for (int x = 0; x < _configuration.Board_With; x++)
            {
                for (int y = 0; y < _configuration.Board_Height; y++)
                {
                    var _boardCell = _cellsFactory.CreateEmptyCell(x, y, _view.GetCellsParent());
                    
                    var cellOffset = _boardCell.GetCellOffset(_configuration.Board_With, _configuration.Board_Height);
                    _boardCell.SetWorldPosition(_view.GetCenterPosition() + cellOffset);
                    
                    _boardCells[x, y] = _boardCell;
                }
            }
        }

        public void Dispose()
        {
            _boardCells = null;
            
            _resourcesProvider.ReleaseResource(_configuration);
        }
    }
}