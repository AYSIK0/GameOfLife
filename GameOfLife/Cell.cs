using System;
namespace GameOfLife
{
    public class Cell
    {
        private int row, columns;
        public int status; // Why do we need status
        public char content;


        public Cell(int r, int c, int stat)
        {
            row = r;
            columns = c;
            status = stat;
            content = ' ';
        }

    }
}
