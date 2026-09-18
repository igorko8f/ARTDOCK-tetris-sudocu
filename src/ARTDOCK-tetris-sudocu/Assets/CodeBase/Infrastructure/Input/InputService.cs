using System;
using CodeBase.Infrastructure.MainCameraService;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CodeBase.Infrastructure.Input
{
    public class InputService : IInputService, IDisposable
    {
        public bool IsInputEnabled => _gameInput.asset.enabled;

        public Observable<Unit> RotatePressed => _rotatePressed;
        public Observable<Unit> PausePressed => _pausePressed;
        public Observable<Unit> MouseClicked => _pausePressed;

        private readonly GameInput _gameInput;
        private readonly ICameraService _cameraService;
        private readonly Subject<Unit> _rotatePressed = new();
        private readonly Subject<Unit> _pausePressed = new();
        private readonly Subject<Unit> _mouseClicked = new();

        public InputService(ICameraService cameraService)
        {
            _cameraService = cameraService;
            _gameInput = new GameInput();

            _gameInput.Gameplay.RotateFigure.performed += OnRotateFigureButtonPressed;
            _gameInput.Gameplay.Pause.performed += OnPauseButtonPressed;
            _gameInput.Gameplay.PointerClick.performed += OnMouseClicked;
            
            DisableInput();
        }

        public void EnableInput() => 
            _gameInput.Enable();

        public void DisableInput() => 
            _gameInput.Disable();

        public Vector2 GetMousePosition()
        {
            var camera = _cameraService.GetMainCamera();
            return camera.ScreenToWorldPoint(_gameInput.Gameplay.MousePosition.ReadValue<Vector2>());
        }

        public void Dispose()
        {
            if (_gameInput != null)
            {
                _gameInput.Gameplay.RotateFigure.performed -= OnRotateFigureButtonPressed;
                _gameInput.Gameplay.Pause.performed -= OnPauseButtonPressed;
                _gameInput.Gameplay.PointerClick.performed -= OnMouseClicked;
            }
            
            _gameInput?.Dispose();
            
            _rotatePressed?.Dispose();
            _pausePressed?.Dispose();
            _mouseClicked?.Dispose();
        }

        private void OnRotateFigureButtonPressed(InputAction.CallbackContext context) => 
            _rotatePressed.OnNext(Unit.Default);

        private void OnPauseButtonPressed(InputAction.CallbackContext obj) => 
            _pausePressed.OnNext(Unit.Default);

        private void OnMouseClicked(InputAction.CallbackContext obj) => 
            _mouseClicked.OnNext(Unit.Default);
    }
}