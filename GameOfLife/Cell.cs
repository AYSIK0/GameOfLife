using System;
namespace GameOfLife
{
    public class Cell
    {
        private int row, column;
        public char content;

        public Cell(int r, int c)
        {
            row = r;
            column = c;
            content = ' ';
        }
    }
}
