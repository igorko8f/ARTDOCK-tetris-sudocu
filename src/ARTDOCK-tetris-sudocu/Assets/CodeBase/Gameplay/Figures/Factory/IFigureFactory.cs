using UnityEngine;

namespace CodeBase.Gameplay.Figures.Factory
{
    public interface IFigureFactory
    {
        Figure CreateFigure(Transform parent);
    }
}