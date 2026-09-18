using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.MainCameraService
{
    public class CameraService : ICameraService
    {
        private Camera _mainCamera;

        public CameraService()
        {
            _mainCamera = Camera.main;
        }

        public Camera GetMainCamera()
        {
            if (_mainCamera == null) 
                _mainCamera = Camera.main;
            
            if (_mainCamera == null)
                _mainCamera = Object.FindObjectOfType<Camera>();
            
            return _mainCamera == null 
                ? throw new ArgumentNullException("Main camera not found!") 
                : _mainCamera;
        }
    }
}