using CodeBase.Infrastructure.Audio;
using CodeBase.Infrastructure.CoroutineRunner;
using CodeBase.Infrastructure.Input;
using CodeBase.Infrastructure.Loading;
using CodeBase.Infrastructure.MainCameraService;
using CodeBase.Infrastructure.ResourcesProvider;
using CodeBase.Infrastructure.SaveLoad;
using CodeBase.Infrastructure.StateMachineService.StateMachine;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private CoroutineRunnerComponent _coroutineRunner;
        [SerializeField] private AudioService _audioService;
        
        public override void InstallBindings()
        {
            BindCoroutineRunner();
            BindSceneLoadingService();
            BindGameStateMachine();
            BindInputService();
            BindCameraService();
            BindProjectResourcesProvider();
            BindSaveService();
            BindAudioService();
        }

        private void BindAudioService()
        {
            Container.Bind<IAudioService>()
                .To<AudioService>()
                .FromComponentInNewPrefab(_audioService)
                .AsSingle()
                .NonLazy();
        }

        private void BindSaveService()
        {
            Container.Bind<ISaveService>()
                .To<SaveService>()
                .AsSingle();
        }

        private void BindProjectResourcesProvider()
        {
            Container.Bind<IProjectResourcesProvider>()
                .To<ProjectResourcesProvider>()
                .AsSingle();
        }

        private void BindCameraService()
        {
            Container.Bind<ICameraService>()
                .To<CameraService>()
                .AsSingle();
        }

        private void BindInputService()
        {
            Container.BindInterfacesTo<InputService>()
                .AsSingle();
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