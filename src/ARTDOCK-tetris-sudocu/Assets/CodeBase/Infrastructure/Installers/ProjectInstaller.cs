using CodeBase.Infrastructure.CoroutineRunner;
using CodeBase.Infrastructure.Loading;
using CodeBase.Infrastructure.StateMachineService.StateMachine;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private CoroutineRunnerComponent _coroutineRunner;
        
        public override void InstallBindings()
        {
            BindCoroutineRunner();
            BindSceneLoadingService();
            BindGameStateMachine();
        }

        private void BindGameStateMachine()
        {
            Container.BindInterfacesTo<GameStateMachine>()
                .AsSingle()
                .NonLazy();
        }
        
        private void BindSceneLoadingService()
        {
            Container.Bind<ISceneLoader>()
                .To<SceneLoader>()
                .AsSingle();
        }
        
        private void BindCoroutineRunner()
        {
            Container.Bind<ICoroutineRunner>()
                .To<CoroutineRunnerComponent>()
                .FromComponentInNewPrefab(_coroutineRunner)
                .AsSingle()
                .NonLazy();
        }
    }
}