using System;
using System.Collections.Generic;
using CodeBase.Gameplay.Cells;
using CodeBase.Gameplay.Cells.Factory;
using CodeBase.Gameplay.Common.Extensions;
using CodeBase.Infrastructure.MainCameraService;
using CodeBase.Infrastructure.ResourcesProvider;
using DG.Tweening;
using R3;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Figures
{
    public class Figure : MonoBehaviour, IResource, IDisposable
    {
        [SerializeField] private FigureUI _figureUI;
        [SerializeField] private Transform _cellsRoot;
        [SerializeField] private float _rotateAnimationDuration = 0.2f;
        
        [SerializeField] private Collider2D _collider;
        [SerializeField] private Rigidbody2D _rigidbody;

        public bool IsDisposed { get; private set; }
        
        private IBoardCellsFactory _boardCellsFactory;
        private CompositeDisposable _compositeDisposable;

        private int MaxSize => FigureConfiguration.Size;
        private int X_Size => _matrix.GetLength(0);
        private int Y_Size => _matrix.GetLength(1);
        
        private Vector3 _initialPosition;
        private Transform _origin;
        
        private bool[,] _matrix;
        private List<BoardCell> _cells;

        [Inject]
        public void Construct(IBoardCellsFactory boardCellsFactory, ICameraService cameraService)
        {
            _boardCellsFactory = boardCellsFactory;
            _compositeDisposable = new CompositeDisposable();
            
            _figureUI.Initialize(cameraService.GetMainCamera());
            
            _figureUI.RotateClockwisePressed
                .Subscribe(_ => Rotate(true))
                .AddTo(_compositeDisposable);
            
            _figureUI.RotateCounterClockwisePressed
                .Subscribe(_ => Rotate(false))
                .AddTo(_compositeDisposable);
            
            _cells = new List<BoardCell>(MaxSize * MaxSize);
        }

        public void Initialize(FigureConfiguration configuration)
        {
            _matrix = configuration.Matrix.CropToBounds();
            
            BuildFigureCells();
            SetInteractableState(true);

            IsDisposed = false;
            _figureUI.Show();
        }

        public void Rotate(bool clockwise)
        {
            _matrix = _matrix.Rotate(clockwise);
            PositionateCells(false);

            _cellsRoot.DOComplete();
            _cellsRoot.DORotate(new Vector3(0, 0, _cellsRoot.eulerAngles.z - ( clockwise ? 90 : -90)), _rotateAnimationDuration)
                .SetEase(Ease.InOutSine);
        }

        public Vector2 GetPosition() => 
            transform.position;

        public bool[,] GetMatrix() => 
            _matrix;

        public void SetInteractableState(bool isInteractable)
        {
            _collider.enabled = isInteractable;
        }

        public void BeforeDraggingPerformed()
        {
            _initialPosition = transform.localPosition;
            _origin = transform.parent;
            _figureUI.Hide();
            
            SetInteractableState(false);
        }

        public void RestorePosition()
        {
            transform.parent = _origin;
            transform.localPosition = _initialPosition;
            _figureUI.Show();
            
            SetInteractableState(true);
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
            
            RestorePosition();
            CleanUpCells();

            IsDisposed = true;
            _figureUI.Hide();
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
    }
}