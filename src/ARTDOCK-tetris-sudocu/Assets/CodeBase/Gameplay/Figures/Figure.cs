using System;
using CodeBase.Infrastructure.Input;
using CodeBase.Infrastructure.ResourcesProvider;
using R3;
using UnityEngine;

namespace CodeBase.Gameplay.Figures
{
    public class Figure : MonoBehaviour, IResource
    {
        private int Size => FigureConfiguration.Size;

        private IInputService _inputService;
        private bool[,] _matrix;

        public CompositeDisposable _dispossable;
        
        public void Initialize(FigureConfiguration configuration,
            IInputService inputService)
        {
            _inputService = inputService;
            _dispossable = new CompositeDisposable();
            
            _matrix = configuration.Matrix;
        }
        
        private void OnDestroy()
        {
            _dispossable?.Dispose();
        }

        private void Rotate()
        {
            
        }
    }
}