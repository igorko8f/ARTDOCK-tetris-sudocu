using System;
using CodeBase.Infrastructure.ResourcesProvider;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Cells.Factory
{
    public class BoardCellsFactory : IBoardCellsFactory, IDisposable
    {
        private readonly IProjectResourcesProvider _resourcesProvider;
        private readonly DiContainer _container;
        private readonly BoardCell _cellPrefab;

        public BoardCellsFactory(IProjectResourcesProvider resourcesProvider,
            DiContainer container)
        {
            _resourcesProvider = resourcesProvider;
            _container = container;
            
            _cellPrefab = _resourcesProvider.LoadResource<BoardCell>();
        }

        public BoardCell CreateEmptyCell(int x, int y, Transform parent)
        {
            var cell = _container.InstantiatePrefabForComponent<BoardCell>(_cellPrefab, parent);
            var data = new BoardCellData(x, y);
            
            cell.Initialize(data);
            return cell;
        }
        
        public BoardCell CreateEmptyCell(Transform parent)
        {
            var cell = _container.InstantiatePrefabForComponent<BoardCell>(_cellPrefab, parent);
            var data = new BoardCellData(-1, -1);
            
            cell.Initialize(data);
            return cell;
        }

        public void Dispose()
        {
        }
    }
}