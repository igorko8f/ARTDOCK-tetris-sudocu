using CodeBase.Gameplay.Board;
using CodeBase.Gameplay.Board.Factory;
using CodeBase.Gameplay.Board.States;
using CodeBase.Gameplay.Cells.Factory;
using CodeBase.Gameplay.Draggables;
using CodeBase.Gameplay.Draggables.Physics;
using CodeBase.Gameplay.Figures.Factory;
using CodeBase.Gameplay.Figures.Tray;
using CodeBase.Gameplay.PlayerScore;
using CodeBase.Gameplay.UI;
using CodeBase.Infrastructure.StateMachineService.StateMachine;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Installers
{
    public class GameplayInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private FigureTray _figureTrayPrefab;
        [SerializeField] private Transform _figureTrayOrigin;
        [SerializeField] private UIPopups _uiPopups;
        
        public override void InstallBindings()
        {
            BindGameplayInstaller();
            BindGameBoard();
            BindGameStateMachine();
            BindFigures();
            BindDraggableService();
            BindPlayerScoreService();
            BindUIPopups();
        }

        public void Initialize()
        {
            Container.Resolve<IGameStateMachine>()
                .Enter<InitializeGameState>();
        }

        private void BindUIPopups()
        {
            Container.Bind<UIPopups>()
                .To<UIPopups>()
                .FromComponentInNewPrefab(_uiPopups)
                .AsSingle()
                .NonLazy();
        }

        private void BindPlayerScoreService()
        {
            Container.BindInterfacesTo<PlayerScoreService>()
                .AsSingle();
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