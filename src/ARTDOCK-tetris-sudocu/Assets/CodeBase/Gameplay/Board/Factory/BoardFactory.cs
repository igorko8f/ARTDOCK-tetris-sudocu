using System;
using CodeBase.Infrastructure.ResourcesProvider;
using Zenject;

namespace CodeBase.Gameplay.Board.Factory
{
    public class BoardFactory : IBoardFactory, IDisposable
    {
        private readonly IProjectResourcesProvider _resourcesProvider;
        private readonly DiContainer _container;
        private readonly GameBoardView _boardViewPrefab;

        public BoardFactory(IProjectResourcesProvider resourcesProvider,
            DiContainer container)
        {
            _resourcesProvider = resourcesProvider;
            _container = container;
            
            _boardViewPrefab = _resourcesProvider.LoadResource<GameBoardView>();
        }

        public GameBoardView CreateBoardView(int width, int height)
        {
            var boardView = _container.InstantiatePrefabForComponent<GameBoardView>(_boardViewPrefab);
            boardView.Initialize(width, height);
            
            return boardView;
        }

        public void Dispose()
        {
            
        }
    }
}