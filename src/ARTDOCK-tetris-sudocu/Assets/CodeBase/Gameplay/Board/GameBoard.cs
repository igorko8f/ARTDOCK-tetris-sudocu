using System;
using CodeBase.Gameplay.Board.Factory;
using CodeBase.Gameplay.Board.States;
using CodeBase.Gameplay.Cells;
using CodeBase.Gameplay.Cells.Factory;
using CodeBase.Gameplay.Common.Extensions;
using CodeBase.Infrastructure.ResourcesProvider;
using CodeBase.Infrastructure.StateMachineService.StateMachine;
using UnityEngine;

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

        public bool TryGetCellAt(Vector2 worldPosition, out (int x, int y) coordinates)
        {
            var localPosition = _view.GetCellsParent().InverseTransformPoint(worldPosition);

            localPosition += new Vector3(
                _configuration.Board_With * _cellsFactory.CellSize * 0.5f,
                _configuration.Board_Height * _cellsFactory.CellSize * 0.5f);
            
            var x = Mathf.FloorToInt(localPosition.x / _cellsFactory.CellSize);
            var y = Mathf.FloorToInt(localPosition.y / _cellsFactory.CellSize);

            coordinates = (x, y);

            return x >= 0 && x < _configuration.Board_With &&
                   y >= 0 && y < _configuration.Board_Height;
        }

        public void ActivateCells(bool[,] figureMatrix, (int x, int y) coordinates, bool previewOnly = false)
        {
            var offsetX = Mathf.FloorToInt(figureMatrix.SizeX() / 2f);
            var offsetY = Mathf.FloorToInt(figureMatrix.SizeY() / 2f);
            
            for (int x = 0; x < figureMatrix.SizeX(); x++)
            {
                for (int y = 0; y < figureMatrix.SizeY(); y++)
                {
                    if (!figureMatrix[x, y])
                        continue;
                    
                    var cellX = coordinates.x + x - offsetX;
                    var cellY = coordinates.y + y - offsetY;

                    if (cellX >= 0 && cellX < _configuration.Board_With &&
                        cellY >= 0 && cellY < _configuration.Board_Height)
                    {
                        if (previewOnly)
                            _boardCells[cellX, cellY].SetPreviewState(true);
                        else
                            _boardCells[cellX, cellY].SetActive();
                    }
                }
            }
        }

        public bool CouldPlaceFigureOn(bool[,] figureMatrix, (int x, int y) coordinates)
        {
            var offsetX = Mathf.FloorToInt(figureMatrix.SizeX() / 2f);
            var offsetY = Mathf.FloorToInt(figureMatrix.SizeY() / 2f);
            
            for (int x = 0; x < figureMatrix.SizeX(); x++)
            {
                for (int y = 0; y < figureMatrix.SizeY(); y++)
                {
                    if (!figureMatrix[x, y])
                        continue;
                    
                    var cellX = coordinates.x + x - offsetX;
                    var cellY = coordinates.y + y - offsetY;

                    if (cellX < 0 || cellX >= _configuration.Board_With ||
                        cellY < 0 || cellY >= _configuration.Board_Height)
                    {
                        return false;
                    }

                    if (_boardCells[cellX, cellY].IsActive())
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void ResetPreviewState()
        {
            for (int x = 0; x < _configuration.Board_With; x++)
            {
                for (int y = 0; y < _configuration.Board_Height; y++)
                {
                    if (_boardCells[x, y].PreviewEnabled() == false || _boardCells[x, y].IsActive())
                        continue;

                    _boardCells[x, y].SetPreviewState(false);
                }
            }
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