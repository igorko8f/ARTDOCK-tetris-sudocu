using System;
using CodeBase.Infrastructure.Input;
using CodeBase.Infrastructure.ResourcesProvider;
using Zenject;

namespace CodeBase.Gameplay.Figures.Factory
{
    public class FigureFactory : IFigureFactory, IDisposable
    {
        private readonly DiContainer _container;
        private readonly Figure _figurePrefab;
        private readonly IInputService _inputService;

        public FigureFactory(IProjectResourcesProvider resourcesProvider,
            IInputService inputService,
            DiContainer container)
        {
            _container = container;
            _inputService = inputService;
            
            _figurePrefab = resourcesProvider.LoadResource<Figure>();
        }

        public Figure CreateFigure(FigureConfiguration configuration)
        {
            var figure = _container.InstantiatePrefabForComponent<Figure>(_figurePrefab);
            figure.Initialize(configuration, _inputService);
            return figure;
        }

        public void Dispose()
        {
            
        }
    }
}