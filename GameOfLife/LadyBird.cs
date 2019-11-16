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
            List<int[]> possibleGfLocations = new List<int[]>();
            int[] possibleGf;
            int[] preyCoor;

            for (int i = 0; i < 4; i++)
            {
                possibleGf = GetNeighbourCoor(i, row, column);

                if (possibleGf[0] >= 0 && possibleGf[0] < grid.GetLength(0) && possibleGf[1] >= 0 && possibleGf[1] < grid.GetLength(1))
                {
                    // Can wes just another and(&&) in the first if.
                    if (grid[possibleGf[0], possibleGf[1]].content == 'o')
                    {
                        possibleGfLocations.Add(possibleGf);
                    }
                }
            }

            if (possibleGfLocations.Count > 0)
            {
                int randCell = rand.Next(0, possibleGfLocations.Count);
                preyCoor = possibleGfLocations[randCell];

                grid[row, column].content = ' ';

                row = preyCoor[0];
                column = preyCoor[1];

                grid[row, column].content = shape;
                notEating = 0;
                eat = true;

            }

            else
            {
                int randDir = rand.Next(0, 4); // Min = 0 || Max = 3.
                int[] nextcell = GetNeighbourCoor(randDir, row, column);

                while (nextcell[0] >= grid.GetLength(0) || nextcell[0] < 0 || nextcell[1] >= grid.GetLength(1) || nextcell[1] < 0)
                {
                    randDir = rand.Next(0, 4);
                    nextcell = GetNeighbourCoor(randDir, row, column);
                }

                if (grid[nextcell[0], nextcell[1]].content == ' ') // if the next cell is empty than move the ladybird.
                {
                    grid[row, column].content = ' '; // X and Y will still be same (orignal location of ladybird).
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

                // We need to make sure that X and Y are between 0 and 20 first

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
                int randBreedCell = rand.Next(0, possibleCells.Count); // Min = 0 || Maxpossible = 0 or 1 or 2 or 3.
                breedCell = possibleCells[randBreedCell];

                LadyBird nextLB = new LadyBird(breedCell[0], breedCell[1]);
                grid[nextLB.row, nextLB.column].content = nextLB.shape;
                ladyBirds.Add(nextLB);

            }

        }
    }
}
