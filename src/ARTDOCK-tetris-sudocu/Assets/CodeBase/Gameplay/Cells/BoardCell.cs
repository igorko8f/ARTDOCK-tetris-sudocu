using CodeBase.Infrastructure.ResourcesProvider;
using UnityEngine;

namespace CodeBase.Gameplay.Cells
{
    public class BoardCell : MonoBehaviour, IResource
    {
        [SerializeField] private BoardCellView _view;
        
        private BoardCellData _data;

        public void Initialize(BoardCellData data)
        {
            _data = data;
            _view.UpdateView(_data);
        }

        public void SetWorldPosition(Vector2 getCenterPosition) => 
            transform.position = getCenterPosition;

        public Vector2 GetCellOffset(int width, int height)
        {
            var offsetX = (width - 1) * 0.5f;
            var offsetY = (height - 1) * 0.5f;

            return new Vector2(
                _data.Position.x - offsetX,
                _data.Position.y - offsetY
            );
        }
    }
}