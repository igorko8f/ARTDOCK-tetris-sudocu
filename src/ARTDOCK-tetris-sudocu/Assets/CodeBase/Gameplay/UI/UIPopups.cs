using CodeBase.Infrastructure.MainCameraService;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.UI
{
    public class UIPopups : MonoBehaviour
    {
        [SerializeField] private UIBase _pausePopup;
        [SerializeField] private UIBase _loseGamePopup;
        [SerializeField] private Canvas _backgroundCanvas;
        
        [Inject]
        private void Construct(ICameraService cameraService) => 
            _backgroundCanvas.worldCamera = cameraService.GetMainCamera();

        public void ShowPausePopup()
        {
            CloseAllPopups();
            _pausePopup.Show();
        }

        public void ShowLoseGamePopup()
        {
            CloseAllPopups();
            _loseGamePopup.Show();
        }

        public void CloseAllPopups()
        {
            _pausePopup.Hide();
            _loseGamePopup.Hide();
        }
    }
}