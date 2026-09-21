using DG.Tweening;
using UnityEngine;

namespace CodeBase.Gameplay.UI
{
    public class UIBase : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _showAnimationDuration = 0.5f;

        private bool _isShown = false;
        
        public virtual void Show()
        {
            if (_isShown) 
                return;
            
            gameObject.SetActive(true);
            _canvasGroup.blocksRaycasts = true;

            _canvasGroup.DOFade(1f, _showAnimationDuration)
                .OnComplete(OnUIShown);
        }

        public virtual void Hide()
        {
            if (_isShown == false)
                return;
            
            _canvasGroup.DOFade(0f, _showAnimationDuration)
                .OnComplete(OnUIHided);
        }

        private void OnUIShown()
        {
            _isShown = true;
            _canvasGroup.interactable = true;
        }

        private void OnUIHided()
        {
            _isShown = false;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
            gameObject.SetActive(false);
        }
    }
}