using System.Collections.Generic;
using CodeBase.Gameplay.Cells;

namespace CodeBase.Gameplay.Board.States.Payloads
{
    public struct CompletedLine
    {
        public List<BoardCell> Line;

        public CompletedLine(List<BoardCell> line)
        {
            Line = line;
        }
    }
}