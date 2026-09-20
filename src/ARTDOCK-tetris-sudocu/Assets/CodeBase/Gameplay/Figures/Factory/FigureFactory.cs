using System;
using CodeBase.Infrastructure.Input;
using CodeBase.Infrastructure.ResourcesProvider;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Figures.Factory
{
    public class FigureFactory : IFigureFactory, IDisposable
    {
        private readonly DiContainer _container;
        private readonly Figure _figurePrefab;

        public FigureFactory(IProjectResourcesProvider resourcesProvider,
            DiContainer container)
        {
            _container = container;
            
            _figurePrefab = resourcesProvider.LoadResource<Figure>();
        }

        public Figure CreateFigure(Transform parent)
        {
            var figure = _container.InstantiatePrefabForComponent<Figure>(_figurePrefab, parent);
            return figure;
        }

        public void Dispose()
        {
            
        }
    }
}