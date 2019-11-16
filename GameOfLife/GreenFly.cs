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
            int randDir = rand.Next(0, 4); // Min = 0 || Max = 3.
            int[] nextcell = GetNeighbourCoor(randDir, row, column);

            while (nextcell[0] >= grid.GetLength(0) || nextcell[0] < 0 || nextcell[1] >= grid.GetLength(1) || nextcell[1] < 0)
            {
                randDir = rand.Next(0, 4);
                nextcell = GetNeighbourCoor(randDir, row, column);
            }


            if (grid[nextcell[0], nextcell[1]].content == ' ') // if the next cell is empty than move the greenfly.
            {
                grid[row, column].content = ' ';
                row = nextcell[0];
                column = nextcell[1];
                grid[row, column].content = shape; 
            }
        }

        public void Breed(Cell[,] grid, List<GreenFly> greenFlies)
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

                GreenFly nextGf = new GreenFly(breedCell[0], breedCell[1]);
                grid[nextGf.row, nextGf.column].content = nextGf.shape;
                greenFlies.Add(nextGf);

            }
        }

        public static void KillGreenFly(List<GreenFly> gflies, int r, int c)
        {
            int index = -1; // may use -1.
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
