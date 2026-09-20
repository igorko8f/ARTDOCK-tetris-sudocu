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

        public void UpdatePosition(int x, int y) => 
            Position = (x, y);
        
        public void SetActive(bool isActive) => 
            IsActive = isActive;

        public void SetPreviewEnabled(bool isPreviewEnabled) => 
            IsPreviewEnabled = isPreviewEnabled;
    }
}