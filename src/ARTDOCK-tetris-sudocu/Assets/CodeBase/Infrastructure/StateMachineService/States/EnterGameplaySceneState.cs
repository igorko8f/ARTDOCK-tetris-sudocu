using CodeBase.Infrastructure.Loading;
using CodeBase.Infrastructure.StateMachineService.StateInfrastructure;
using CodeBase.Infrastructure.StateMachineService.StateMachine;
using CodeBase.StaticData;

namespace CodeBase.Infrastructure.StateMachineService.States
{
    public class EnterGameplaySceneState : SimpleState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;

        public EnterGameplaySceneState(IGameStateMachine stateMachine,
            ISceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }

        public override void Enter()
        {
            base.Enter();
            _sceneLoader.LoadScene(Scenes.GameplaySceneInfo.Name);
        }
    }
}