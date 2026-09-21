using CodeBase.Gameplay.Draggables;
using CodeBase.Gameplay.Figures.Tray;
using CodeBase.Infrastructure.Audio;
using CodeBase.Infrastructure.Input;
using CodeBase.Infrastructure.StateMachineService.StateInfrastructure;

namespace CodeBase.Gameplay.Board.States
{
    public class PauseState : SimpleState
    {
        private readonly IInputService _inputService;
        private readonly IAudioService _audioService;
        private readonly FigureTray _tray;
        private readonly IDraggableService _draggableService;
        private readonly GameBoard _gameBoard;

        public PauseState(IInputService inputService,
            IAudioService audioService,
            IDraggableService draggableService,
            GameBoard gameBoard,
            FigureTray tray)
        {
            _inputService = inputService;
            _audioService = audioService;
            _draggableService = draggableService;
            _gameBoard = gameBoard;
            _tray = tray;
        }

        public override void Enter()
        {
            base.Enter();
            
            _audioService.SetMusicVolume(0.5f);
            
            var currentFigure = _draggableService.CurrentDraggableFigure;
            if (currentFigure != null)
            {
                _draggableService.ReleaseDraggable();
                currentFigure.RestorePosition();
            }
            
            _gameBoard.ResetPreviewState();
            
            var remainedFigures = _tray.GetRemainedFigures();
            foreach (var figure in remainedFigures) 
                figure.SetInteractableState(false);
            
            _inputService.EnableInput();
        }

        protected override void Exit()
        {
            _audioService.SetMusicVolume(1f);
            
            var remainedFigures = _tray.GetRemainedFigures();
            foreach (var figure in remainedFigures) 
                figure.SetInteractableState(true);
            
            base.Exit();
        }
    }
}