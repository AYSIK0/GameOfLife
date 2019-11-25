using System;
namespace GameOfLife
{
    public class Cell
    {
        private int row, column;
        public int status; // Why do we need status!!!
        public char content;


        public Cell(int r, int c, int stat)
        {
            row = r;
            column = c;
            status = stat;
            content = ' ';
        }

        public void ChangeStatus()
        {
            if (status == 3)
            {
                content = ' ';
            }
        }

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
