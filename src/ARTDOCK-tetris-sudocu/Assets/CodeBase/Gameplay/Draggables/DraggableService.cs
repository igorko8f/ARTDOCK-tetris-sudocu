using System;
using CodeBase.Gameplay.Board.States;
using CodeBase.Gameplay.Draggables.Physics;
using CodeBase.Gameplay.Figures;
using CodeBase.Infrastructure.Audio;
using CodeBase.Infrastructure.Input;
using CodeBase.Infrastructure.ResourcesProvider;
using CodeBase.Infrastructure.StateMachineService.StateMachine;
using R3;
using UnityEngine;

namespace CodeBase.Gameplay.Draggables
{
    public class DraggableService : IDraggableService, IDisposable
    {
        public Figure CurrentDraggableFigure => _currentDraggable;
        
        private readonly IInputService _inputService;
        private readonly IPhysicsInteractions _physicsInteractions;
        private readonly IGameStateMachine _stateMachine;
        private readonly CompositeDisposable _compositeDisposable;
        private readonly IAudioService _audioService;
        private readonly SoundsConfig _soundsConfig;

        private Figure _currentDraggable;
        private Vector3 _dragOffset;

        public DraggableService(IInputService inputService, 
            IPhysicsInteractions physicsInteractions,
            IGameStateMachine stateMachine,
            IProjectResourcesProvider resourcesProvider,
            IAudioService audioService)
        {
            _inputService = inputService;
            _physicsInteractions = physicsInteractions;
            _stateMachine = stateMachine;
            _audioService = audioService;
            _soundsConfig = resourcesProvider.LoadResource<SoundsConfig>();
            
            _compositeDisposable = new CompositeDisposable();
            
            _inputService.RotatePressed
                .Subscribe(_ => OnRotatePressed())
                .AddTo(_compositeDisposable);
            
            _inputService.MouseClicked
                .Subscribe(_ => OnMouseClicked())
                .AddTo(_compositeDisposable);
            
            _inputService.MouseReleased
                .Subscribe(_ => OnMouseReleased())
                .AddTo(_compositeDisposable);
            
            Observable.EveryUpdate()
                .Subscribe(_ => UpdateDrag())
                .AddTo(_compositeDisposable);
        }

        public void ReleaseDraggable()
        {
            _currentDraggable = null;
            PlayPlaceSFX();
        }

        private void OnRotatePressed()
        {
            if (IsDragging() == false)
                return;

            _currentDraggable.Rotate(true);
        }

        private void OnMouseClicked()
        {
            if (IsDragging())
                return;

            var worldPosition = _inputService.GetMouseWorldPosition();

            if (_physicsInteractions.GetObjectAtPoint<Figure>(worldPosition, out var draggable))
            {
                SetDraggable(draggable);
                _stateMachine.Enter<FigurePreviewRenderState>();
            }
        }

        private void OnMouseReleased()
        {
            if (IsDragging())
               _stateMachine.Enter<PlaceFigureState>();
        }

        private void UpdateDrag()
        {
            if (IsDragging() == false)
                return;

            var mousePosition = _inputService.GetMouseWorldPosition();
            _currentDraggable.transform.position = mousePosition + _dragOffset;
        }

        private void SetDraggable(Figure draggable)
        {
            _currentDraggable = draggable;
            _dragOffset = _currentDraggable.transform.position - _inputService.GetMouseWorldPosition();
            _currentDraggable.BeforeDraggingPerformed();
            
            PlayTakeSFX();
        }

        private void PlayTakeSFX() => 
            _audioService.PlaySfx(_soundsConfig.TakeSFX);

        private void PlayPlaceSFX() => 
            _audioService.PlaySfx(_soundsConfig.PlaceSFX);

        private bool IsDragging() =>
            _currentDraggable != null;

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
    }
}