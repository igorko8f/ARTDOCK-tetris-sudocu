using CodeBase.Gameplay.Figures;
using UnityEngine;

namespace CodeBase.Gameplay.Draggables.Physics
{
    public interface IPhysicsInteractions
    {
        bool GetObjectAtPoint<T>(Vector2 worldPosition, out T draggedFigure) where T : Component;
    }
}