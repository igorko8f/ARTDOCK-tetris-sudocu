namespace CodeBase.Gameplay.Cells
{
    public class BoardCellData
    {
        public bool IsActive { get; private set; } = false;
        public bool IsPreviewEnabled { get; private set; } = false;
        public (int x, int y) Position { get; private set; } = (0, 0);

        public BoardCellData(int x, int y)
        {
            IsActive = false;
            IsPreviewEnabled = false;
            Position = (x, y);
        }
    }
}