using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Gameplay.Board.Factory;
using CodeBase.Gameplay.Board.States;
using CodeBase.Gameplay.Board.States.Payloads;
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

        public IEnumerable<(int x, int y)> GetBoardPositionsAccordingToFigure(bool[,] figureMatrix, (int x, int y) coordinates)
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

                    yield return (cellX, cellY);
                }
            }
        }
        
        public void ActivateCells(IEnumerable<(int x, int y)> boardPositions, bool previewOnly = false)
        {
            foreach (var (x, y) in boardPositions)
            {
                if (IsInsideBoard(x, y) == false)
                    continue;
                
                if (previewOnly)
                    _boardCells[x, y].SetPreviewState(true);
                else
                    _boardCells[x, y].SetActive();
            }
        }

        public bool CouldPlaceFigureOn(IEnumerable<(int x, int y)> boardPositions)
        {
            foreach (var (x, y) in boardPositions)
            {
                if (IsInsideBoard(x, y) == false)
                    return false;
                
                if (_boardCells[x, y].IsActive())
                    return false;
            }
            
            return true;
        }

        public List<CompletedLine> GetCompletedLines(IEnumerable<(int x, int y)> boardPositions)
        {
            var completedLines = new List<CompletedLine>();
            var checkedRows = new HashSet<int>();
            var checkedColumns = new HashSet<int>();

            foreach (var (x, y) in boardPositions)
            {
                if (checkedRows.Add(y) && IsRowCompleted(y))
                {
                    var cells = GetRowCells(y, x);
                    completedLines.Add(new CompletedLine(cells.ToList()));
                }

                if (checkedColumns.Add(x) && IsColumnCompleted(x))
                {
                    var cells = GetColumnCells(x, y);
                    completedLines.Add(new CompletedLine(cells.ToList()));
                }
            }

            return completedLines;
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

        private bool IsInsideBoard(int x, int y)
        {
            return x >= 0 && x < _configuration.Board_With &&
                   y >= 0 && y < _configuration.Board_Height;
        }
        
        private bool IsRowCompleted(int y)
        {
            for (var x = 0; x < _configuration.Board_With; x++)
            {
                if (!_boardCells[x, y].IsActive())
                    return false;
            }

            return true;
        }
        
        private bool IsColumnCompleted(int x)
        {
            for (var y = 0; y < _configuration.Board_Height; y++)
            {
                if (!_boardCells[x, y].IsActive())
                    return false;
            }

            return true;
        }
        
        private IEnumerable<BoardCell> GetRowCells(int y, int startX)
        {
            yield return _boardCells[startX, y];
            
            for (var distance = 1; distance < _configuration.Board_With; distance++)
            {
                var left = startX - distance;

                if (left >= 0)
                    yield return _boardCells[left, y];

                var right = startX + distance;

                if (right < _configuration.Board_With)
                    yield return _boardCells[right, y];
            }
        }
        
        private IEnumerable<BoardCell> GetColumnCells(int x, int startY)
        {
            yield return _boardCells[x, startY];
            
            for (var distance = 1; distance < _configuration.Board_Height; distance++)
            {
                var bottom = startY - distance;

                if (bottom >= 0)
                    yield return _boardCells[x, bottom];

                var top = startY + distance;

                if (top < _configuration.Board_Height)
                    yield return _boardCells[x, top];
            }
        }
        
        public void Dispose()
        {
            _boardCells = null;
            
            _resourcesProvider.ReleaseResource(_configuration);
        }
    }
}