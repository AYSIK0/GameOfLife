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
            List<int[]> preyLocations = new List<int[]>(); 
            int[] preyCoor;

            for (int i = 0; i < 4; i++)
            {
                preyCoor = GetNeighbourCoor(i, row, column);

                if (preyCoor[0] >= 0 && preyCoor[0] < grid.GetLength(0) && preyCoor[1] >= 0 && preyCoor[1] < grid.GetLength(1))
                {
                    // Can wes just another and(&&) in the first if.
                    if (grid[preyCoor[0], preyCoor[1]].content == 'o')
                    {
                        preyLocations.Add(preyCoor);
                    }
                }
            }

            if (preyLocations.Count > 0)
            {
                int randCell = rand.Next(0, preyLocations.Count);
                preyCoor = preyLocations[randCell];

                grid[row, column].content = ' ';

                row = preyCoor[0];
                column = preyCoor[1];

                grid[row, column].content = shape;
                notEating = 0;
                eat = true;

            }

            else
            {
                int randDir = rand.Next(0, 4); // Min = 0 ||| Max = 3.
                int[] nextcell = GetNeighbourCoor(randDir, row, column);

                while (nextcell[0] >= grid.GetLength(0) || nextcell[0] < 0 || nextcell[1] >= grid.GetLength(1) || nextcell[1] < 0)
                {
                    randDir = rand.Next(0, 4);
                    nextcell = GetNeighbourCoor(randDir, row, column);
                }

                if (grid[nextcell[0], nextcell[1]].content == ' ') // if the next cell is empty than move the ladybird.
                {
                    grid[row, column].content = ' '; // Emptying the orignal location of ladybird since it moved.
                    row = nextcell[0];
                    column = nextcell[1];
                    grid[row, column].content = shape;
                }

                notEating++;
                eat = false;
            }
        }

        public void Breed(Cell[,] grid, List<LadyBird> ladyBirds)
        {
            List<int[]> possibleCells = new List<int[]>();
            int[] breedCell;

            for (int i = 0; i < 4; i++)
            {
                breedCell = GetNeighbourCoor(i, row, column);

                // Checks if the cell exist within the grid (row and column are between 0 and 19)
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

                LadyBird nextLB = new LadyBird(breedCell[0], breedCell[1]);
                grid[nextLB.row, nextLB.column].content = nextLB.shape;
                ladyBirds.Add(nextLB);

            }

        }
    }
}
