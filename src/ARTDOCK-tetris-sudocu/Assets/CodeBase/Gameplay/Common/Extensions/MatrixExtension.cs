using System.Linq;
using UnityEngine;

namespace CodeBase.Gameplay.Common.Extensions
{
    public static class MatrixExtension
    {
        public static bool HasActiveNeighbour(this bool[,] matrix, int x, int y)
        {
            return matrix.IsActive(x - 1, y)
                   || matrix.IsActive(x + 1, y)
                   || matrix.IsActive(x, y - 1)
                   || matrix.IsActive(x, y + 1);
        }

        public static bool IsActive(this bool[,] matrix, int x, int y)
        {
            if (x > matrix.GetLength(0) - 1 || x < 0)
                return false;
            
            if (y > matrix.GetLength(1) - 1 || y < 0)
                return false;
            
            return matrix[x, y];
        }

        public static bool IsEmpty(this bool[,] matrix) => 
            !matrix.Cast<bool>()
                .Any(x => x);

        public static int OccupiedCells(this bool[,] matrix) => 
            matrix.Cast<bool>()
                .Count(cell => cell);
        
        public static int ActiveNeighboursCount(this bool[,] matrix, int x, int y)
        {
            var count = 0;
            if (matrix.IsActive(x - 1, y)) count++;
            if (matrix.IsActive(x + 1, y)) count++;
            if (matrix.IsActive(x, y - 1)) count++;
            if (matrix.IsActive(x, y + 1)) count++;
            return count;
        }
    }
}