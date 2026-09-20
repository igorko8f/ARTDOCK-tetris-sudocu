using CodeBase.Gameplay.Board;
using CodeBase.Gameplay.Board.Factory;
using CodeBase.Gameplay.Board.States;
using CodeBase.Gameplay.Cells.Factory;
using CodeBase.Gameplay.Draggables;
using CodeBase.Gameplay.Draggables.Physics;
using CodeBase.Gameplay.Figures.Factory;
using CodeBase.Gameplay.Figures.Tray;
using CodeBase.Infrastructure.StateMachineService.StateMachine;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Installers
{
    public class GameplayInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private FigureTray _figureTrayPrefab;
        [SerializeField] private Transform _figureTrayOrigin;
        
        public override void InstallBindings()
        {
            BindGameplayInstaller();
            BindGameBoard();
            BindGameStateMachine();
            BindFigures();
            BindDraggableService();
        }

        public void Initialize()
        {
            Container.Resolve<IGameStateMachine>()
                .Enter<InitializeGameState>();
        }

        private void BindDraggableService()
        {
            Container.BindInterfacesTo<DraggableService>()
                .AsSingle();
            
            Container.Bind<IPhysicsInteractions>()
                .To<PhysicsInteractions>()
                .AsSingle();
        }

        private void BindFigures()
        {
            Container.Bind<FigureTray>()
                .To<FigureTray>()
                .FromComponentInNewPrefab(_figureTrayPrefab)
                .AsSingle()
                .WithArguments(_figureTrayOrigin)
                .NonLazy();
            
            Container.BindInterfacesTo<FigureFactory>()
                .AsSingle();
        }

        private void BindGameplayInstaller()
        {
            Container.BindInterfacesTo<GameplayInstaller>()
                .FromInstance(this)
                .AsSingle();
        }

        private void BindGameStateMachine()
        {
            Container.BindInterfacesTo<GameStateMachine>()
                .AsSingle();
        }

        private void BindGameBoard()
        {
            Container.BindInterfacesTo<BoardFactory>()
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesTo<BoardCellsFactory>()
                .AsSingle()
                .NonLazy();
            
            Container.BindInterfacesAndSelfTo<GameBoard>()
                .AsSingle()
                .NonLazy();
        }
    }
}