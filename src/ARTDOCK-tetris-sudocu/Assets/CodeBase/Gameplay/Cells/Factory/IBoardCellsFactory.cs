using UnityEngine;

namespace CodeBase.Gameplay.Cells.Factory
{
    public interface IBoardCellsFactory
    {
        float CellSize { get; }
        BoardCell CreateEmptyCell(int i, int j, Transform parent);
        BoardCell CreateEmptyCell(Transform parent);
    }
}