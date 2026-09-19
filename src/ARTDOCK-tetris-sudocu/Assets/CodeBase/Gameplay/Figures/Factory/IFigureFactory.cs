namespace CodeBase.Gameplay.Figures.Factory
{
    public interface IFigureFactory
    {
        Figure CreateFigure(FigureConfiguration configuration);
    }
}