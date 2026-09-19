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
        [SerializeField] private Transform _trayCenter;
        
        private IProjectResourcesProvider _resourcesProvider;
        private IFigureFactory _figureFactory;

        [Inject]
        public void Construct(Transform origin, 
            IProjectResourcesProvider resourcesProvider,
            IFigureFactory figureFactory)
        {
            transform.position = origin.position;
            _resourcesProvider = resourcesProvider;
            _figureFactory = figureFactory;
        }

        public void BuildTray()
        {
            
        }
        
    }
}