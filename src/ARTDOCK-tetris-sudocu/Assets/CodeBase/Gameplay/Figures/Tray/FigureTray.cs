using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Gameplay.Common.Extensions;
using CodeBase.Gameplay.Figures.Factory;
using CodeBase.Infrastructure.ResourcesProvider;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Figures.Tray
{
    public class FigureTray : MonoBehaviour
    {
        public const int TraySize = 3;
        
        [SerializeField] private Transform _trayRoot;
        [SerializeField] private float _figureXOffset = 1f;
        
        private List<Figure> _figures = new(TraySize);
        
        private IProjectResourcesProvider _resourcesProvider;
        private IFigureFactory _figureFactory;
        
        private List<FigureConfiguration> _figuresConfigs;

        private int _currentTrayElements;
        
        [Inject]
        public void Construct(Transform origin, 
            IProjectResourcesProvider resourcesProvider,
            IFigureFactory figureFactory)
        {
            transform.position = origin.position;
            _resourcesProvider = resourcesProvider;
            _figureFactory = figureFactory;
            
            _figuresConfigs = _resourcesProvider.LoadResources<FigureConfiguration>()
                .ToList();
        }

        private void OnDestroy()
        {
            
        }

        public void RemoveFigure(Figure figure)
        {
            figure.Dispose();
            _currentTrayElements -= 1;
            
            if (_currentTrayElements <= 0)
            {
                BuildTray();
            }
        }

        public void BuildTray()
        {
            BuildNewFigures();
            InitializeFigures();
            
            _currentTrayElements = TraySize;
        }

        private void InitializeFigures()
        {
            var selectedConfigs = new List<FigureConfiguration>();
            var xFigureSize = FigureConfiguration.Size + 1;
            
            for (int i = 0; i < TraySize; i++)
            {
                var randomConfig = _figuresConfigs.PickRandom(selectedConfigs);
                selectedConfigs.Add(randomConfig);
                
                var position = (i * (xFigureSize + _figureXOffset)) - xFigureSize;
                _figures[i].transform.localPosition = new Vector3(position, 0, 0);
                
                _figures[i].Initialize(randomConfig);
            }
        }

        private void BuildNewFigures()
        {
            var needed = TraySize - _figures.Count;
            for (int i = 0; i < needed; i++)
            {
                var figure = _figureFactory.CreateFigure(_trayRoot);
                _figures.Add(figure);
            }
        }

        public IEnumerable<Figure> GetRemainedFigures() => 
            _figures.Where(figure => !figure.IsDisposed);
    }
}