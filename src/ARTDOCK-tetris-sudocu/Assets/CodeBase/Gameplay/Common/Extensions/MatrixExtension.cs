using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeBase.Gameplay.Common.Extensions
{
    public static class MatrixExtension
    {
        public static int SizeX(this bool[,] matrix) =>
            matrix.GetLength(0);
        
        public static int SizeY(this bool[,] matrix) => 
            matrix.GetLength(1);

        public static IEnumerable<(int x, int y, bool state)> GetActiveNeighbours(this bool[,] matrix, int x, int y)
        {
            var neighbours = new List<(int x, int y, bool state)>();
            
            if (matrix.IsActive(x - 1, y)) neighbours.Add((x - 1, y, true));
            if (matrix.IsActive(x + 1, y)) neighbours.Add((x + 1, y, true));
            if (matrix.IsActive(x, y - 1)) neighbours.Add((x, y - 1, true));
            if (matrix.IsActive(x, y + 1)) neighbours.Add((x, y + 1, true));
            
            return neighbours;
        }

        public static bool HasActiveNeighbour(this bool[,] matrix, int x, int y)
        {
            return GetActiveNeighbours(matrix, x, y)
                .Any();
        }

        public static int ActiveNeighboursCount(this bool[,] matrix, int x, int y)
        {
            return GetActiveNeighbours(matrix, x, y)
                .Count();
        }

        public static bool IsActive(this bool[,] matrix, int x, int y)
        {
            return IsInside(matrix, x, y) && matrix[x, y];
        }

        public static bool IsEmpty(this bool[,] matrix) => 
            !matrix.Cast<bool>()
                .Any(x => x);

        public static int OccupiedCells(this bool[,] matrix) => 
            matrix.Cast<bool>()
                .Count(cell => cell);
        
        private static (int x, int y, bool state) FindFirstActiveCell(bool[,] matrix)
        {
            for (var x = 0; x < matrix.SizeX(); x++)
            {
                for (var y = 0; y < matrix.SizeY(); y++)
                {
                    if (matrix[x, y])
                        return (x, y, true);
                }
            }

            return (0, 0, false);
        }
        
        private static bool IsInside(bool[,] matrix, int x, int y)
        {
            return x >= 0 &&
                   x < matrix.SizeX() &&
                   y >= 0 &&
                   y < matrix.SizeY();
        }
        
        public static bool IsConnected(this bool[,] matrix)
        {
            var start = FindFirstActiveCell(matrix);
            
            if (start == (0, 0, false))
                return true;

            var visited = new bool[matrix.SizeX(), matrix.SizeY()];

            var queue = new Queue<(int x, int y, bool state)>();
            queue.Enqueue(start);
            visited[start.x, start.y] = true;

            var visitedCount = 0;

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                visitedCount++;

                foreach (var neighbour in GetActiveNeighbours(matrix, current.x, current.y))
                {
                    if (!matrix[neighbour.x, neighbour.y])
                        continue;

                    if (visited[neighbour.x, neighbour.y])
                        continue;

                    visited[neighbour.x, neighbour.y] = true;
                    queue.Enqueue(neighbour);
                }
            }

            return visitedCount == matrix.OccupiedCells();
        }
        
        public static bool[,] Rotate(this bool[,] matrix, bool clockwise)
        {
            var rows = matrix.SizeX();
            var cols = matrix.SizeY();
            
            var rotatedMatrix = new bool[cols, rows];

            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    rotatedMatrix[
                        clockwise ? col : cols - 1 - col, 
                        clockwise ? rows - 1 - row : row] 
                        = matrix[row, col];
                }
            }
            
            return rotatedMatrix.CropToBounds();
        }
        
        public static bool[,] CropToBounds(this bool[,] matrix)
        {
            var rows = matrix.SizeX();
            var cols = matrix.SizeY();

            var minRow = rows;
            var maxRow = -1;
            var minCol = cols;
            var maxCol = -1;

            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    if (!matrix[row, col])
                        continue;
                    
                    minRow = Math.Min(minRow, row);
                    maxRow = Math.Max(maxRow, row);
                    minCol = Math.Min(minCol, col);
                    maxCol = Math.Max(maxCol, col);
                }
            }

            if (maxRow == -1)
                return new bool[0, 0];

            var newMatrix = new bool[maxRow - minRow + 1, maxCol - minCol + 1];

            for (var row = minRow; row <= maxRow; row++)
            {
                for (var col = minCol; col <= maxCol; col++)
                {
                    newMatrix[row - minRow, col - minCol] = matrix[row, col];
                }
            }

            return newMatrix;
        }
    }
}