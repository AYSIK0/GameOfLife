using System;
using System.Collections.Generic;

namespace GameOfLife
{
    public class LadyBird : Creature
    {
        public int notEating;
        public bool eat;
        public LadyBird(int r, int c) : base(r, c)
        {
            eat = false;
            shape = 'x';
            notEating = 0;
        }

        public void Move(Cell[,] grid)
        {
            // [1] (For loop) First we check all the surrounding cells (up, down, left and right) for greenflies (prey),
            //     if a cell has a prey it will be added to the "preyLocations" list.

            // [2] (if part) If the list isn't empty a random cell will be selected,
            //     then the ladybird eat the greenfly and move to this cell.

            // [3] (else part) When there are no prey in nearby cells a random cell will be selected to move to .

            List<int[]> preyLocations = new List<int[]>(); 
            int[] preyCoor;

            // [1]
            for (int i = 0; i < 4; i++)
            {
                preyCoor = GetNeighbourCoor(i, row, column);

                if (preyCoor[0] >= 0 && preyCoor[0] < grid.GetLength(0) && preyCoor[1] >= 0 && preyCoor[1] < grid.GetLength(1))
                {
                    if (grid[preyCoor[0], preyCoor[1]].content == 'o')
                    {
                        preyLocations.Add(preyCoor);
                    }
                }
            }

            // [2]
            if (preyLocations.Count > 0)
            {
                int randCell = rand.Next(0, preyLocations.Count);
                preyCoor = preyLocations[randCell];

                grid[row, column].content = ' '; // Emptying the orignal location of ladybird since it moved.

                row = preyCoor[0];
                column = preyCoor[1];

                grid[row, column].content = shape;
                notEating = 0;
                eat = true;

            }

            // [3]
            else
            {
                int randDir = rand.Next(0, 4); // Min = 0 ||| Max = 3.
                int[] nextcell = GetNeighbourCoor(randDir, row, column);

                // If the cell is outside the grid we keep looping until we find a cell inside the grid.
                // This approach has a problem if the grid has only 1 cell, it will get stuck in a loop.
                while (nextcell[0] >= grid.GetLength(0) || nextcell[0] < 0 || nextcell[1] >= grid.GetLength(1) || nextcell[1] < 0)
                {
                    randDir = rand.Next(0, 4);
                    nextcell = GetNeighbourCoor(randDir, row, column);
                }

                if (grid[nextcell[0], nextcell[1]].content == ' ') // if the next cell is empty then move the ladybird.
                {
                    grid[row, column].content = ' '; // Emptying the original location of ladybird since it moved.
                    row = nextcell[0];
                    column = nextcell[1];
                    grid[row, column].content = shape; // Changing the shape of cell to the shape of ladybird.
                }

                notEating++;
                eat = false;
            }
        }

        public void Breed(Cell[,] grid, List<LadyBird> ladyBirds)
        {
            //This method handle the breed part for ladybird, it has two main parts:

            //[1] (for loop) it checks the four cells (up, down, right, left) close to the ladybird and add empty cells to the list.
            //[2] (if part) if empty cells were found, a random cell is chosen to create a new ladybird in it.

            List<int[]> possibleCells = new List<int[]>();
            int[] breedCell;

            for (int i = 0; i < 4; i++)
            {
                breedCell = GetNeighbourCoor(i, row, column);

                // Checks if the cell exist within the grid (row and column)
                if ((breedCell[0] < grid.GetLength(0) && breedCell[0] >= 0) && (breedCell[1] < grid.GetLength(1) && breedCell[1] >= 0))
                {
                    if (grid[breedCell[0], breedCell[1]].content == ' ')
                    {
                        possibleCells.Add(breedCell);
                    }
                }
            }

            if (possibleCells.Count > 0)
            {
                int randBreedCell = rand.Next(0, possibleCells.Count); // Min = 0 ||| Maxpossible = 0 or 1 or 2 or 3.
                breedCell = possibleCells[randBreedCell];

                LadyBird nextLb = new LadyBird(breedCell[0], breedCell[1]);
                grid[nextLb.row, nextLb.column].content = nextLb.shape;
                ladyBirds.Add(nextLb);

            }

        }
    }
}
