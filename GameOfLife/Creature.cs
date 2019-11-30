using System;

namespace GameOfLife
{
    public class Creature
    {
        public int row, column;
        public int lifeTime;
        public char shape;
        public Random rand = new Random();

        public Creature(int r, int c)
        {
            row = r;
            column = c;
            lifeTime = 0;

        }

        public int[] GetNeighbourCoor(int direction, int row, int column)
        {
            // This method return the coordinates of neighbour cells using a direction and coordinates of a creature.

            int neighX = 0;
            int neighY = 0;

            if (direction == 0) // Up
            {
                neighX = row - 1;
                neighY = column;
            }
            else if (direction == 1) //Down
            {
                neighX = row + 1;
                neighY = column;
            }
            else if (direction == 2) //Right
            {
                neighY = column + 1;
                neighX = row;
            }
            else if (direction == 3) // Left
            {
                neighY = column - 1;
                neighX = row;
            }

            return new int[] { neighX, neighY }; 
        }

    }
}
