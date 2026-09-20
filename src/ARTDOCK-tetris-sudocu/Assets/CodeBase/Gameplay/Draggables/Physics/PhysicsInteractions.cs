using CodeBase.Gameplay.Cells;
using CodeBase.Gameplay.Figures;
using UnityEngine;

namespace CodeBase.Gameplay.Draggables.Physics
{
    public class PhysicsInteractions : IPhysicsInteractions
    {
        public PhysicsInteractions()
        {
        }

        public bool GetObjectAtPoint<T>(Vector2 worldPosition, out T draggedFigure) where T : Component
        {
            var hit = Physics2D.OverlapPoint(worldPosition);

            if (hit == null)
            {
                draggedFigure = null;
                return false;
            }

            draggedFigure = hit.GetComponentInParent<T>();
            return draggedFigure != null;
        }
    }
}