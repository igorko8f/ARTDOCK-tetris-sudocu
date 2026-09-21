using CodeBase.Gameplay.Figures.Tray;
using CodeBase.Infrastructure.Audio;
using CodeBase.Infrastructure.ResourcesProvider;
using CodeBase.Infrastructure.StateMachineService.StateInfrastructure;
using CodeBase.Infrastructure.StateMachineService.StateMachine;
using UnityEngine;

namespace CodeBase.Gameplay.Board.States
{
    public class InitializeGameState : SimpleState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IProjectResourcesProvider _resourcesProvider;
        private readonly IAudioService _audioService;
        private readonly GameBoard _gameBoard;
        private readonly FigureTray _figureTray;

        public InitializeGameState(IGameStateMachine stateMachine,
            IProjectResourcesProvider resourcesProvider,
            IAudioService audioService,
            GameBoard gameBoard,
            FigureTray figureTray)
        {
            _stateMachine = stateMachine;
            _resourcesProvider = resourcesProvider;
            _audioService = audioService;
            _gameBoard = gameBoard;
            _figureTray = figureTray;
        }

        public override void Enter()
        {
            base.Enter();
            
            var soundConfig = _resourcesProvider.LoadResource<SoundsConfig>();
            _audioService.PlayMusic(soundConfig.BackgroundClip);
            
            _gameBoard.BuildBoard();
            _figureTray.BuildTray();

            //Going to checking lose state to prevent case when board size is too small for figures to be placed
            _stateMachine.Enter<CheckLoseState>();
        }
    }
}