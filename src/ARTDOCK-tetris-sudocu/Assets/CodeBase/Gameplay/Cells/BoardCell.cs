using CodeBase.Infrastructure.ResourcesProvider;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Gameplay.Cells
{
    public class BoardCell : MonoBehaviour, IResource
    {
        [SerializeField] private BoardCellView _view;
        [SerializeField] private float DestroyAnimationInterval = 0.01f;
        
        private BoardCellData _data;

        public void Initialize(BoardCellData data)
        {
            _data = data;
            _view.UpdateView(_data);
        }

        public void Show() => 
            gameObject.SetActive(true);

        public void Hide() => 
            gameObject.SetActive(false);

        public void SetWorldPosition(Vector2 getCenterPosition) => 
            transform.position = getCenterPosition;

        public void UpdatePosition(int x, int y) => 
            _data.UpdatePosition(x, y);

        public void SetActive()
        {
            _data.SetActive(true);
            _view.UpdateView(_data);
        }

        public void SetPreviewState(bool isEnabled)
        {
            _data.SetPreviewEnabled(isEnabled);
            _view.UpdateView(_data);
        }

        public void Cleanup(bool forgetLocation = true)
        {
            _data.SetActive(false);
            _data.SetPreviewEnabled(false);
            
            if (forgetLocation)
                _data.UpdatePosition(-1, -1);
            
            _view.UpdateView(_data);
        }

        public Vector2 GetCellOffset(int width, int height)
        {
            var offsetX = (width - 1) * 0.5f;
            var offsetY = (height - 1) * 0.5f;

            return new Vector2(
                _data.Position.x - offsetX,
                _data.Position.y - offsetY
            );
        }
        
        public float GetCellSize() => 
            transform.localScale.x;

        public bool IsActive() => 
            _data.IsActive;

        public bool PreviewEnabled() => 
            _data.IsPreviewEnabled;

        public Tween DestroyAnimation()
        {
            var sequence = DOTween.Sequence();
            sequence.SetTarget(gameObject);
            sequence.SetAutoKill(true);
            
            sequence.AppendCallback(() => Cleanup(false));
            sequence.AppendInterval(DestroyAnimationInterval);

            return sequence;
        }
    }
}