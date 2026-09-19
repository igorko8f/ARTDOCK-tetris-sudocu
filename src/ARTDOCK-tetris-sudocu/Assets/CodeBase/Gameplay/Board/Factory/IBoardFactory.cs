namespace CodeBase.Gameplay.Board.Factory
{
    public interface IBoardFactory
    {
        GameBoardView CreateBoardView(int width, int height);
    }
}