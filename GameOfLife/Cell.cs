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

        //public void ChangeStatus()
        //{

        //}

        //public void ChangeContent()
        //{
        //    if (status == 1) // Greenfly
        //    {
        //        content = GreenFly.shape;
        //    }

        //    else if (status == 2) // LadyBird
        //    {
        //        content = LadyBird.shape;
        //    }

        //    else if (status == 3)
        //    {
        //        content = ' ';
        //    }
        //}

    }
}
