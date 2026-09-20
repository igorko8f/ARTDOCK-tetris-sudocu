using CodeBase.Gameplay.Figures;

namespace CodeBase.Gameplay.Draggables
{
    public interface IDraggableService
    {
        Figure CurrentDraggableFigure { get; }
        void ReleaseDraggable();
    }
}