using CodeBase.Infrastructure.StateMachineService.StateMachine;
using CodeBase.Infrastructure.StateMachineService.States;
using Zenject;

namespace CodeBase.Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller, IInitializable
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<BootstrapInstaller>()
                .FromInstance(this)
                .AsSingle();
        }

        public void Initialize()
        {
            Container.Resolve<IGameStateMachine>()
                .Enter<BootstrapState>();
        }
    }
}