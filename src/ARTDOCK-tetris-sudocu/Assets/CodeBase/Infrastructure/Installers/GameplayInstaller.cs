using CodeBase.Gameplay.Board;
using CodeBase.Gameplay.Board.Factory;
using CodeBase.Gameplay.Board.States;
using CodeBase.Gameplay.Cells.Factory;
using CodeBase.Infrastructure.StateMachineService.StateMachine;
using Zenject;

namespace CodeBase.Infrastructure.Installers
{
    public class GameplayInstaller : MonoInstaller, IInitializable
    {
        public override void InstallBindings()
        {
            BindGameplayInstaller();
            BindGameBoard();
            BindGameStateMachine();
        }

        public void Initialize()
        {
            Container.Resolve<IGameStateMachine>()
                .Enter<InitializeGameState>();
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