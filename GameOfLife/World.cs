using System;
using System.Collections.Generic;
using System.IO;

namespace GameOfLife
{
    public class World
    {
        private int id; 
        private static int nextId = 1;
        private int width, length;
        public Cell[,] cells;
        public int timeStep;
        private int startGreenflyNums, startLadybirdNums;
        private List<GreenFly> greenflies = new List<GreenFly>();
        private List<LadyBird> ladyBirds = new List<LadyBird>();
        private static string filePath;

        // This attribute will be the general information about the world.
        private int totalNumOfGf = 0, totalNumOfLb = 0;
        private int highestNumOfGf = 0, highestNumOfLb = 0;
        private int lowestNumOfGf, lowestNumOfLb;
        private float avergaeNumOfGf = 0, avergaeNumOfLb =  0;

        public World(int L, int W, int GFs, int LBs)
        {
            this.id = nextId++;
            filePath = filePath;
            length = L;
            width = W;
            startGreenflyNums = GFs;
            startLadybirdNums = LBs;
            lowestNumOfGf = startGreenflyNums;
            lowestNumOfLb = startLadybirdNums;
        }

        public int Width
        {
            get { return width; }
        }

        public void Draw() // Draws The world.
        {
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                Console.Write("|");

                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    if (j != cells.GetLength(1) - 1)
                    {
                        Console.Write(cells[i, j].content);
                    }
                    else
                    {
                        Console.WriteLine(cells[i, j].content + "|");
                        
                    }
                } // j
            } // i
        }

        public void Generate() // This method Create the world each turn.
        {
            /// The method generate the world, it has two parts:
            /// 1. When the world is empty (else part).
            /// 2. When the world has been craeted (if part) the method will call other methods to make the neccessary changes.

            if (cells != null) // [2]
            {
                ChangeLadybirds();
                ChangeGreenflies();
            }

            else //[1]
            {
                timeStep = 0;
                cells = new Cell[length, width];
                for (int i = 0; i < cells.GetLength(0); i++)
                {
                    for (int j = 0; j < cells.GetLength(1); j++)
                    {
                        cells[i, j] = new Cell(i, j);
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

        private void ChangeGreenflies() // This method Move and Breed GreenFlies.
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

        private void ChangeLadybirds() // This method Move and Breed Ladybirds.
        {
            for (int L = ladyBirds.Count - 1; L >= 0; L--)
            {
                LadyBird currentLb = ladyBirds[L];
                currentLb.lifeTime++;

                if (currentLb.notEating == 3) // Checking if the ladybird has starved.
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

                // BREED Part For LadyBirds.
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

        public int[] GetTheCounts() // Return the current number of Greenflies and Ladybirds in the grid.
        {
            int G = greenflies.Count;
            int L = ladyBirds.Count;
            return new int[2] { G, L };
        }

        public void Information() // This method update the Information.
        {
            totalNumOfGf += greenflies.Count;
            totalNumOfLb += ladyBirds.Count;

            if (highestNumOfLb < ladyBirds.Count)
            {
                highestNumOfLb = ladyBirds.Count;
            }

            if (highestNumOfGf < greenflies.Count)
            {
                highestNumOfGf = greenflies.Count;
            }

            if (lowestNumOfGf > greenflies.Count)
            {
                lowestNumOfGf = greenflies.Count;
            }

            if (lowestNumOfLb > ladyBirds.Count)
            {
                lowestNumOfLb = ladyBirds.Count;
            }

            if (timeStep > 0) 
            {
                avergaeNumOfGf = Convert.ToSingle(totalNumOfGf) / Convert.ToSingle(timeStep);
                avergaeNumOfLb = Convert.ToSingle(totalNumOfLb) / Convert.ToSingle(timeStep);
            }
            else
            {
                avergaeNumOfGf = Convert.ToSingle(totalNumOfGf);
                avergaeNumOfLb = Convert.ToSingle(totalNumOfLb);
            }
        }

        public void WriteToFile(bool createNewFile) 
        {
            //This method write information about the world in a text file.

            string fileName, currentDir, pathString;
            if (createNewFile == true || filePath == null) 
            {
                int fileNumber = 0;
                // We keep genrating file names until one of them doen't already exist in the current directory.
                do
                {
                    fileName = $"Information{fileNumber++}.txt";
                    currentDir = Directory.GetCurrentDirectory();
                    pathString = Path.Combine(currentDir, fileName);

                } while (File.Exists(pathString));
                filePath = pathString;
            }

            StreamWriter file = File.AppendText(filePath);
            file.WriteLine("General Information About World {0}.\n", id);
            file.WriteLine("Number of Steps: " + timeStep);
            file.WriteLine("Grid Size: {0}x{1}", length, width);
            file.WriteLine("Start Number for Greenflies: {0}", startGreenflyNums);
            file.WriteLine("Start Number for Ladybirds: {0}", startLadybirdNums);
            file.WriteLine("Highest Number of Greenflies: " + highestNumOfGf);
            file.WriteLine("Highest Number of Ladybirds: " + highestNumOfLb);
            file.WriteLine("Average number of Greenflies per turn: " + avergaeNumOfGf);
            file.WriteLine("Average number of Ladybirds per turn: " + avergaeNumOfLb);
            file.WriteLine("Lowest number of Greenflies: {0}", lowestNumOfGf);
            file.WriteLine("Lowest number of Ladybirds: {0}", lowestNumOfLb);
            file.WriteLine("\n");
            file.Close();
        }

    }
}
