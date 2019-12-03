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
        private static string[] filePath;
        private Dictionary<int, int[]> graphValues = new Dictionary<int, int[]>(); 

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
                graphValues.Add(timeStep, new int[] { greenflies.Count, ladyBirds.Count });
            }

            else //[1]
            {
                timeStep = 0;
                graphValues.Add(timeStep, new int[] { startGreenflyNums, startLadybirdNums });
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

            string infoFileName, tableFileName, currentDir, pathString1, pathString2;
            int fileNumber = 0;
            if (createNewFile == true || filePath == null)
            {
                // We keep genrating file names until one of them doen't already exist in the current directory.
                do
                {
                    fileNumber++;
                    infoFileName = $"Information{fileNumber}.txt";
                    tableFileName = $"Table{fileNumber}.txt";
                    currentDir = Directory.GetCurrentDirectory();
                    pathString1 = Path.Combine(currentDir, infoFileName);
                    pathString2 = Path.Combine(currentDir, tableFileName);

                } while (File.Exists(pathString1) || File.Exists(pathString2));
                filePath = new string[2]{ pathString1, pathString2 };
            }

            // Inforamtion File.
            StreamWriter infoFile = File.AppendText(filePath[0]);
            infoFile.WriteLine("General Information About World {0}.\n", id);
            infoFile.WriteLine("Number of Steps: " + timeStep);
            infoFile.WriteLine("Grid Size: {0}x{1}", length, width);
            infoFile.WriteLine("Start Number for Greenflies: {0}", startGreenflyNums);
            infoFile.WriteLine("Start Number for Ladybirds: {0}", startLadybirdNums);
            infoFile.WriteLine("Highest Number of Greenflies: " + highestNumOfGf);
            infoFile.WriteLine("Highest Number of Ladybirds: " + highestNumOfLb);
            infoFile.WriteLine("Average number of Greenflies per turn: " + avergaeNumOfGf);
            infoFile.WriteLine("Average number of Ladybirds per turn: " + avergaeNumOfLb);
            infoFile.WriteLine("Lowest number of Greenflies: {0}", lowestNumOfGf);
            infoFile.WriteLine("Lowest number of Ladybirds: {0}", lowestNumOfLb);
            infoFile.WriteLine("\n");
            infoFile.Close();

            // Table File.
            StreamWriter tableFile = File.AppendText(filePath[1]);
            tableFile.WriteLine("Table for World {0}.\n", id);
            tableFile.WriteLine(new string('-', 30));
            tableFile.WriteLine("TimeStep  |Greenfly  |Ladybird");
            tableFile.WriteLine(new string('-', 30));
            
            foreach (KeyValuePair<int, int[]> item in graphValues)
            {
                tableFile.WriteLine(item.Key.ToString().PadRight(10) + "|" + item.Value[0].ToString().PadRight(10)
                                + "|" + item.Value[1].ToString().PadRight(10));
            }

            tableFile.WriteLine(new string('-', 30));
            tableFile.WriteLine("\n");
            tableFile.Close();
            
        }

    }
}
