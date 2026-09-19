using UnityEngine;

namespace CodeBase.Gameplay.Cells
{
    public class BoardCellView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        [SerializeField] private Sprite _activeSprite;
        [SerializeField] private Sprite _previewSprite;
        [SerializeField] private Sprite _disabledSprite;
        
        public void UpdateView(BoardCellData data)
        {
            if (data.IsActive)
            {
                _spriteRenderer.sprite = _activeSprite;
                return;
            }
            
            if (data.IsPreviewEnabled)
            {
                _spriteRenderer.sprite = _previewSprite;
                return;
            }

            _spriteRenderer.sprite = _disabledSprite;
        }
    }
}