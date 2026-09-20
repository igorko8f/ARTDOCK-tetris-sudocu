using System;
using System.Collections.Generic;
using CodeBase.Gameplay.Cells;
using CodeBase.Gameplay.Cells.Factory;
using CodeBase.Gameplay.Common.Extensions;
using CodeBase.Infrastructure.Input;
using CodeBase.Infrastructure.ResourcesProvider;
using DG.Tweening;
using R3;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Figures
{
    public class Figure : MonoBehaviour, IResource, IDisposable
    {
        [SerializeField] private Transform _cellsRoot;
        [SerializeField] private float _rotateAnimationDuration = 0.2f;

        private IInputService _inputService;
        private IBoardCellsFactory _boardCellsFactory;
        private CompositeDisposable _dispossable;

        private int MaxSize => FigureConfiguration.Size;
        private int X_Size => _matrix.GetLength(0);
        private int Y_Size => _matrix.GetLength(1);
        
        private bool[,] _matrix;
        private List<BoardCell> _cells;

        [Inject]
        public void Construct(IInputService inputService,
            IBoardCellsFactory boardCellsFactory)
        {
            _inputService = inputService;
            _boardCellsFactory = boardCellsFactory;
            
            _cells = new List<BoardCell>(MaxSize * MaxSize);
        }

        public void Initialize(FigureConfiguration configuration)
        {
            _dispossable = new CompositeDisposable();
            _matrix = configuration.Matrix.CropToBounds();
            
            _inputService.RotatePressed
                .Subscribe(_ => Rotate())
                .AddTo(_dispossable);

            BuildFigureCells();
        }

        private void OnDestroy() => 
            Dispose();

        public void Dispose()
        {
            _dispossable?.Dispose();
            CleanUpCells();
        }

        private void CleanUpCells()
        {
            foreach (var cell in _cells)
            {
                cell.Cleanup();
                cell.Hide();
            }
        }

        private void BuildFigureCells()
        {
            BuildAvailableCells();
            PositionateCells();
            ActivateCells();
        }

        private void BuildAvailableCells()
        {
            var cellsToGenerate = Mathf.Max(0, (X_Size * Y_Size) - _cells.Count);
            for (int i = 0; i < cellsToGenerate; i++)
            {
                var newCell = _boardCellsFactory.CreateEmptyCell(_cellsRoot);
                _cells.Add(newCell);
            }
        }

        private void PositionateCells(bool changeViewPosition = true)
        {
            for (int x = 0; x < X_Size; x++)
            {
                for (int y = 0; y < Y_Size; y++)
                {
                    var _boardCell = _cells[x * Y_Size + y];
                    _boardCell.UpdatePosition(x, y);

                    var cellOffset = _boardCell.GetCellOffset(X_Size, Y_Size);
                    if (changeViewPosition)
                        _boardCell.SetWorldPosition(GetCellsRootPosition() + cellOffset);
                }
            }
        }

        private void ActivateCells()
        {
            for (int x = 0; x < X_Size; x++)
            {
                for (int y = 0; y < Y_Size; y++)
                {
                    var _boardCell = _cells[x * Y_Size + y];
                    if (!_matrix[x, y])
                    {
                        _boardCell.Hide();
                        continue;
                    }
                    
                    _boardCell.SetActive();
                    _boardCell.Show();
                }
            }
        }

        private Vector2 GetCellsRootPosition() => 
            _cellsRoot.position;

        private void Rotate()
        {
            _matrix = _matrix.RotateClockwise();
            PositionateCells(false);
            
            _cellsRoot.DORotate(new Vector3(0, 0, _cellsRoot.eulerAngles.z - 90), _rotateAnimationDuration)
                .SetEase(Ease.InOutSine);
        }
    }
}