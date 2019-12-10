using System;
using System.Collections.Generic;

namespace GameOfLife
{
    public class GreenFly : Creature
    {
        
        public GreenFly(int r, int c) : base(r, c)
        {
            shape = 'o';
        }

        public void Move(Cell[,] grid)
        {
            /// [1] choose a random cell.
            /// [2] if the cell is outside the grid we keep looping until we find a cell inside the grid.
            ///     this approach has a problem if the grid has only 1 cell, it will get stuck in a loop.
            /// [3] if the chosen cell is empty then move there.

            //[1]
            int randDir = rand.Next(0, 4); // Min = 0 ||| Max = 3.
            int[] nextcell = GetNeighbourCoor(randDir, row, column);

            //[2]
            while (nextcell[0] >= grid.GetLength(0) || nextcell[0] < 0 || nextcell[1] >= grid.GetLength(1) || nextcell[1] < 0)
            {
                randDir = rand.Next(0, 4);
                nextcell = GetNeighbourCoor(randDir, row, column);
            }

            //[3]
            if (grid[nextcell[0], nextcell[1]].content == ' ')
            {
                grid[row, column].content = ' ';
                row = nextcell[0];
                column = nextcell[1];
                grid[row, column].content = shape; 
            }
        }

        public void Breed(Cell[,] grid, List<GreenFly> greenFlies)
        {
            //This method handle the breed part for greenfly, it has two main parts:

            //[1] (for loop) it checks the four cells (up, down, right, left) close to the greenfly and add empty cells to the list.
            //[2] (if part) if empty cells were found, a random cell is chosen to create a new greenfly in it.

            List<int[]> possibleCells = new List<int[]>();
            int[] breedCell;

            for (int i = 0; i < 4; i++) //[1]
            {
                breedCell = GetNeighbourCoor(i, row, column);

                // Checks if the cell exist within the grid.
                if ((breedCell[0] < grid.GetLength(0) && breedCell[0] >= 0) && (breedCell[1] < grid.GetLength(1) && breedCell[1] >= 0))
                {
                    if (grid[breedCell[0], breedCell[1]].content == ' ')
                    {
                        possibleCells.Add(breedCell);
                    }
                }
            }

            if (possibleCells.Count > 0) // [2]
            {
                int randBreedCell = rand.Next(0, possibleCells.Count); // Min = 0 || Maxpossible = 0 or 1 or 2 or 3.
                breedCell = possibleCells[randBreedCell];

                GreenFly nextGf = new GreenFly(breedCell[0], breedCell[1]);
                grid[nextGf.row, nextGf.column].content = nextGf.shape;
                greenFlies.Add(nextGf);

            }
        }

        public static void KillGreenFly(List<GreenFly> gflies, int r, int c)
        {
            // This method finds and remove a greenfly from the greenfly list.
            // It's called when a ladybird eats a greenfly.

            int index = -1; 
            for (int i = 0; i < gflies.Count; i++)
            {
                if (gflies[i].row == r && gflies[i].column == c)
                {
                    index = i;
                    break;
                }
            }
            gflies.RemoveAt(index);
        }

    }
}
