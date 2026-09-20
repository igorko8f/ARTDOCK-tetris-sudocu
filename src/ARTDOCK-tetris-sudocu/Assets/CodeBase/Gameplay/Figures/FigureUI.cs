using R3;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Gameplay.Figures
{
    public class FigureUI : MonoBehaviour
    {
        public Observable<Unit> RotateClockwisePressed => _rotateClockwiseButton.OnClickAsObservable();
        public Observable<Unit> RotateCounterClockwisePressed => _rotateCounterClockwiseButton.OnClickAsObservable();

        [SerializeField] private Canvas _canvas;
        [SerializeField] private Button _rotateClockwiseButton;
        [SerializeField] private Button _rotateCounterClockwiseButton;
        
        public void Initialize(Camera mainCamera) => 
            _canvas.worldCamera = mainCamera;
    }
}