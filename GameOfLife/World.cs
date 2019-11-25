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
 
        int highestNumOfGf = 0, highestNumOfLd = 0;
        float avergaeNumOfGf = 0, avergaeNumOfLd = 0;

        public World(int L, int W, int GFs, int LBs)
        {
            length = L;
            width = W;
            startGreenflyNums = GFs;
            startLadybirdNums = LBs;
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
            /// The method generate the world, it has two parts:
            /// 1. When the world is empty (else part).
            /// 2. When the world has been craeted (if part) the method will call other methods to make the neccessary changes.

            if (cells != null)
            {
                ChangeLadybirds();
                ChangeGreenflies();
            }

            else
            {
                timeStep = 0;
                cells = new Cell[length, width];
                for (int i = 0; i < cells.GetLength(0); i++)
                {
                    for (int j = 0; j < cells.GetLength(1); j++)
                    {
                        cells[i, j] = new Cell(i, j, 3);
                    }
                }

                // GreenFlies and Ladybirds are created randomly throughout the grid.

                Random rand = new Random();
                for (int L = 0; L < startLadybirdNums; L++) // Creating Ladybirds.
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
                        while (!CellIsEmpty(randX, randY)) // keep looping until en empty cell is found.
                        {
                            randX = rand.Next(0, length);
                            randY = rand.Next(0, width);
                        }
                        LadyBird ladyBird = new LadyBird(randX, randY);
                        ladyBirds.Add(ladyBird);
                        cells[randX, randY].content = ladyBird.shape;
                    }
                }

                for (int G = 0; G < startGreenflyNums; G++) //Creating Greenflies.
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
            Information();
        }

        public void ChangeGreenflies()
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

        public void ChangeLadybirds()
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

        public int[] CheckTheCount() // Return the number of greenflies and ladybirds.
        {
            int G = greenflies.Count;
            int L = ladyBirds.Count;
            return new int[2] { G, L };
        }

        public void Information()
        {

            if (highestNumOfLd < ladyBirds.Count)
            {
                highestNumOfLd = ladyBirds.Count;
            }

            if (highestNumOfGf < greenflies.Count)
            {
                highestNumOfGf = greenflies.Count;
            }

            //if (timeStep > 0) // needs more work
            //{
            //    avergaeNumOfGf = Convert.ToSingle(highestNumOfGf) / Convert.ToSingle(timeStep);
            //    avergaeNumOfLd = Convert.ToSingle(highestNumOfLd) / Convert.ToSingle(timeStep);
            //}
        }

        public void WriteToFile()
        {
            //NOt Working.!!!!
            string fileName = "Information.txt";
            string currentDir = Directory.GetCurrentDirectory();
            string pathString = Path.Combine(currentDir, fileName);

            StreamWriter file = new StreamWriter(pathString);
            file.WriteLine("General Information About The simulation\n");
            file.WriteLine("Number of Steps: " + timeStep);
            file.WriteLine("Highest Number of Greenflies: " + highestNumOfGf);
            file.WriteLine("Highest Number of Ladybirds: " + highestNumOfLd);
            //file.WriteLine("Average number of Greenflies per turn: " + avergaeNumOfGf);
            //file.WriteLine("Average number of Ladybirds per turn: " + avergaeNumOfLd);

            //file.WriteLine(currentDir + "    " + pathString);
            file.Close();
        }

    }
}
