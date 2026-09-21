using CodeBase.Infrastructure.Loading;
using CodeBase.Infrastructure.StateMachineService.StateInfrastructure;
using CodeBase.StaticData;

namespace CodeBase.Gameplay.Board.States
{
    public class RestartState : SimpleState
    {
        private readonly ISceneLoader _sceneLoader;

        public RestartState(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public override void Enter()
        {
            base.Enter();
            
            _sceneLoader.RestartScene(Scenes.GameplaySceneInfo.Name);
        }
    }
}