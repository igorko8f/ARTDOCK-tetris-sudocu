using CodeBase.Infrastructure.ResourcesProvider;
using UnityEngine;

namespace CodeBase.Gameplay.Board
{
    public class GameBoardView : MonoBehaviour, IResource
    {
        [SerializeField] private Transform _boardCenter;
        [SerializeField] private SpriteRenderer _boardBorders;
        [SerializeField] private Transform _cellsParent;
        
        public void Initialize(int width, int height)
        {
            _boardBorders.transform.position = _boardCenter.position;
            _boardBorders.size = new Vector2(width / 2, height / 2);

            _cellsParent.position = _boardCenter.position;
        }

        public Transform GetCellsParent() => 
            _cellsParent;

        public Vector2 GetCenterPosition() => 
            _boardCenter.position;
    }
}