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
        public bool continu = true;
        private int startGreenflyNums, startLadybirdNums;
        private List<GreenFly> greenflies = new List<GreenFly>();
        private List<LadyBird> ladybirds = new List<LadyBird>();

        private static string[] filePaths;
        private Dictionary<int, int[]> graphValues = new Dictionary<int, int[]>();
        private static int fileNumber = 0;
        

        // This attribute will be the general information about the world.
        private int totalNumOfGf = 0, totalNumOfLb = 0;
        private int highestNumOfGf = 0, highestNumOfLb = 0;
        private int lowestNumOfGf, lowestNumOfLb;
        private float avergaeNumOfGf = 0f, avergaeNumOfLb = 0f;

        public World(int L, int W, int GFs, int LBs)
        {
            id = nextId++;
            filePaths = filePaths;
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
                graphValues.Add(timeStep, new int[] { greenflies.Count, ladybirds.Count });

                continu &= (ladybirds.Count != 0 && greenflies.Count != 0); // Checking if the number of greenflies or ladybirds has reachead 0
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
                        ladybirds.Add(ladyBird);
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
                        ladybirds.Add(ladyBird);
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

            Information(); // Update the information.
        }

        private void ChangeGreenflies() // This method Move and Breed GreenFlies.
        {
            // Storing the current number of greenflies so we don't loop over new greenflies until next turn.
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
            // We iterate starting at the end of the list for two reasons:
            // 1.If a ladybird is removed we don't want to try an access an element that doesn't exist.
            // 2.When new ladybirds are added we won't loop over them until next turn.

            for (int L = ladybirds.Count - 1; L >= 0; L--)
            {
                LadyBird currentLb = ladybirds[L];
                currentLb.lifeTime++;

                if (currentLb.notEating == 3) // Checking if the ladybird has starved.
                {
                    cells[currentLb.row, currentLb.column].content = ' ';
                    ladybirds.Remove(currentLb);
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
                    currentLb.Breed(cells, ladybirds);
                }

            }
        }

        private bool CellIsEmpty(int x, int y)
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
            int L = ladybirds.Count;
            return new int[2] { G, L };
        }

        private void Information() // This method update the Information.
        {
            totalNumOfGf += greenflies.Count;
            totalNumOfLb += ladybirds.Count;

            if (highestNumOfLb < ladybirds.Count) // Highest Number of Ladybirds.
            {
                highestNumOfLb = ladybirds.Count;
            }

            if (highestNumOfGf < greenflies.Count) // Highest Number of Greenflies.
            {
                highestNumOfGf = greenflies.Count;
            }

            if (lowestNumOfGf > greenflies.Count) // Lowest Number of Ladybirds.
            {
                lowestNumOfGf = greenflies.Count;
            }

            if (lowestNumOfLb > ladybirds.Count) // Highest Number of Ladybirds.
            {
                lowestNumOfLb = ladybirds.Count;
            }

            if (timeStep > 0) 
            {
                avergaeNumOfGf = Convert.ToSingle(totalNumOfGf) / Convert.ToSingle(timeStep); // Average Number of Greenflies per turn.
                avergaeNumOfLb = Convert.ToSingle(totalNumOfLb) / Convert.ToSingle(timeStep); // Average Number of Ladybirds per turn.
            }
            else // When Time Step = 0
            {
                avergaeNumOfGf = Convert.ToSingle(totalNumOfGf);
                avergaeNumOfLb = Convert.ToSingle(totalNumOfLb);
            }
        }

        public void WriteInfoFile()
        {
            //This method write information about the world in a text file.

            string infoFileName, currentDir, infoPathString;
            if (filePaths == null)
            {
                // We keep genrating file names until one of them doen't already exist in the current directory.
                do
                {
                    fileNumber++;
                    infoFileName = $"Information{fileNumber}.txt";
                    currentDir = Directory.GetCurrentDirectory();
                    infoPathString = Path.Combine(currentDir, infoFileName);

                } while (File.Exists(infoPathString));
                filePaths = new string[2];
                filePaths[0] =  infoPathString;
            }

            // Inforamtion File.
            using (StreamWriter infoFile = File.AppendText(filePaths[0]))
            {
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
            }
        }

        public void WriteTableFile()
        {
            // This method will create a csv file for each world.

            string tableFileName, currentDir, tablePathString;

            tableFileName = $"Table{fileNumber}w{id}.csv";
            currentDir = Directory.GetCurrentDirectory();
            tablePathString = Path.Combine(currentDir, tableFileName);
            filePaths[1] = tablePathString;

            // Table File.
            using (StreamWriter tableFile = File.AppendText(filePaths[1]))
            {
                tableFile.WriteLine("TimeStep,Greenfly,Ladybird");

                foreach (KeyValuePair<int, int[]> item in graphValues)
                {
                    tableFile.WriteLine(item.Key.ToString() + "," + item.Value[0].ToString() + "," + item.Value[1].ToString());
                }
            }
        }
    }
}
