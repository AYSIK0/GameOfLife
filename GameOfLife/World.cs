using System;
using System.Collections.Generic;
using System.IO;

namespace GameOfLife
{
    public class World
    {
        private int width, length;
        public Cell[,] cells;
        public int timeStep;
        private int startGreenflyNums, startLadybirdNums;
        private List<GreenFly> greenflies = new List<GreenFly>();
        private List<LadyBird> ladyBirds = new List<LadyBird>();

        public World(int L, int W, int GFs, int LBs)
        {
            length = L;
            width = W;
            startGreenflyNums = GFs;
            startLadybirdNums = LBs;
            timeStep = -1;
        }

        public int Width
        {
            get { return width; }
        }

        public void Draw()
        {
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                Console.Write("|");
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    if (j == cells.GetLength(1) - 1)
                    {
                        Console.WriteLine(cells[i, j].content + "|");
                    }
                    else
                    {
                        Console.Write(cells[i, j].content);
                    }
                }
            }
        }

        public void Generate()
        {
            // The "if" part run only the first time when the grid is empty.
            // GreenFlies and Ladybirds are created randomly.

            if (cells == null)
            {
                cells = new Cell[length, width];
                for (int i = 0; i < cells.GetLength(0); i++)
                {
                    for (int j = 0; j < cells.GetLength(1); j++)
                    {
                        Cell c = new Cell(i, j, 3);
                        cells[i, j] = c;
                    }
                }

                Random rand = new Random();

                for (int L = 0; L < startLadybirdNums; L++)
                {
                    int randX = rand.Next(0, length);
                    int randY = rand.Next(0, width);

                    if (CellIsEmpty(randX, randY))
                    {
                        LadyBird ladyBird = new LadyBird(randX, randY);
                        ladyBirds.Add(ladyBird);
                        cells[randX, randY].content = ladyBird.shape;
                    }
                    else
                    {
                        while (!CellIsEmpty(randX, randY))
                        {
                            randX = rand.Next(0, length);
                            randY = rand.Next(0, width);
                        }
                        LadyBird ladyBird = new LadyBird(randX, randY);
                        ladyBirds.Add(ladyBird);
                        cells[randX, randY].content = ladyBird.shape;
                    }
                }

                for (int G = 0; G < startGreenflyNums; G++)
                {
                    int randX = rand.Next(0, length);
                    int randY = rand.Next(0, width);

                    if (CellIsEmpty(randX, randY))
                    {
                        GreenFly greenfly = new GreenFly(randX, randY);
                        greenflies.Add(greenfly);
                        cells[randX, randY].content = greenfly.shape;

                    }
                    else
                    {
                        while (!CellIsEmpty(randX, randY))
                        {
                            randX = rand.Next(0, length);
                            randY = rand.Next(0, width);
                        }
                        GreenFly greenfly = new GreenFly(randX, randY);
                        greenflies.Add(greenfly);
                        cells[randX, randY].content = greenfly.shape;
                    }
                }

            }

            else
            {
                ChangeLadyBirds();
                ChangeGreenFlies();
            }

            //information();
        }

        public void ChangeGreenFlies()
        {
            int gfNums = greenflies.Count;
            for (int G = 0; G < gfNums; G++)
            {
                GreenFly currentGf = greenflies[G];
                currentGf.lifeTime++;

                // MOVE PART For GreenFlies.
                currentGf.Move(cells);

                // BREED PART For GreenFlies.
                if (currentGf.lifeTime != 0 && currentGf.lifeTime % 3 == 0)
                {
                    currentGf.Breed(cells, greenflies);
                }
            }
        }

        public void ChangeLadyBirds()
        {
            for (int L = ladyBirds.Count - 1; L >= 0; L--)
            {
                LadyBird currentLb = ladyBirds[L];
                currentLb.lifeTime++;

                if (currentLb.notEating == 3)
                {
                    cells[currentLb.row, currentLb.column].content = ' '; // I can also change the state to 3(Empty)
                    ladyBirds.Remove(currentLb);
                    continue;
                }

                // MOVE Part for LadyBirds.
                currentLb.Move(cells);
                if (currentLb.eat)
                {
                    GreenFly.KillGreenFly(greenflies, currentLb.row, currentLb.column);
                }

                // BREED PART For LadyBirds.
                if (currentLb.lifeTime != 0 && currentLb.lifeTime % 8 == 0)
                {
                    currentLb.Breed(cells, ladyBirds);
                }

            }
        }

        public bool CellIsEmpty(int x, int y)
        {
            if (cells[x, y] == null || cells[x, y].content == ' ')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int[] checkTheCount() 
        {
            int G = greenflies.Count;
            int L = ladyBirds.Count;
            return new int[2] { G, L };
        }

        // ADD Information method.
    }
}
