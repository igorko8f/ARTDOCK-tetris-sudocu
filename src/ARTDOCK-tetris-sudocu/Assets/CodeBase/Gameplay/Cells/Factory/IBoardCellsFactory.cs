using UnityEngine;

namespace CodeBase.Gameplay.Cells.Factory
{
    public interface IBoardCellsFactory
    {
        BoardCell CreateEmptyCell(int i, int j, Transform parent);
        BoardCell CreateEmptyCell(Transform parent);
    }
}