using System;
using System.IO;

namespace GameOfLife
{
    public class Game
    {
        private World world;

        private byte startOption;
        private uint rows, cols, gfs, lbs, numsOfCells, numsOfCreatures;
        private uint simulationSpeed, mode;
        private bool createNewFile = true;

        public uint SimulationSpeed
        {
            get { return simulationSpeed; }
            set
            {
                if (value != 0)
                {
                    simulationSpeed = value;
                }

            }
        }

        public uint Mode
        {
            get { return mode; }
            set
            {
                if (value == 0 || value == 1)
                {
                    mode = value;
                }

            }
        }

        public void Start()
        {
            // General Information about the game.
            Console.WriteLine("This a Simulation of the Game of Life, the main parts of it are: ");
            Console.WriteLine("1.The world which is a grid of cells, each cell can either contain a Ladybird, Greenfly or be empty.");
            Console.WriteLine("2.Ladybirds 'x' ,can eat greenflies or move each turn, if it didn't eat any greenflies in the last 3 turns it dies. it can breed after 8 turns.");
            Console.WriteLine("3.Greenflies 'o', can only moves and breed after 3 turns.");
            Console.WriteLine("4.Make sure that the total number of cell is equal or bigger than the number of greenflies and ladybirds combined!!");
            Console.WriteLine("5.Simulation Speed must be bigger than 0.");
            Console.WriteLine("6.There are only Two Display Modes 0 (Grid is drawn every time) and 1 (Only the final grid is drawn).");
            Console.WriteLine("7.You can choose between 2 start options 0: Default and 1: Custom");
            Console.WriteLine("-----------------------------------------------------------------------------------------------------");

            bool startOptionIsValid;
            // This loop make sure that start option is either 0 or 1.
            do
            {
                startOptionIsValid = true;
                try
                {
                    Console.Write("\nChoose Start Option 0 (default) or 1 (custom): ");
                    startOption = Convert.ToByte(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("You can only choose 0 or 1 !!");
                    startOptionIsValid = false;
                }

            } while (!startOptionIsValid || startOption != 0 && startOption != 1);

            if (startOption == 0) // Default Start
            {
                rows = 20;
                cols = 20;
                gfs = 100;
                lbs = 5;
                SimulationSpeed = 1;
                Mode = 0;
            }

            else // Custom Start
            {
                // Rule1 (inputsCorrect): All the inputs must be in the right form.
                // Rule2 (cellsLessThanCreatures): The number of cells is less than the number of all creatures (Greenfliers + Ladybirds)
                bool inputsCorrect, cellsLessThanCreatures;
                do
                {
                    Console.Title = "Game Of Life";
                    Console.WriteLine();
                    Console.WriteLine();
                    try
                    {
                        Console.Write("Enter number of Rows: ");
                        rows = Convert.ToUInt32(Console.ReadLine());

                        Console.Write("Enter number of Columns: ");
                        cols = Convert.ToUInt32(Console.ReadLine());

                        Console.Write("Enter number of GreenFlies: ");
                        gfs = Convert.ToUInt32(Console.ReadLine());

                        Console.Write("Enter number of LadyBirds: ");
                        lbs = Convert.ToUInt32(Console.ReadLine());

                        Console.Write("Enter simulation speed: ");
                        simulationSpeed = Convert.ToUInt32(Console.ReadLine());

                        Console.Write("Choose Display mode(0 or 1): ");
                        mode = Convert.ToUInt32(Console.ReadLine());

                        inputsCorrect = true;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Rows, columns, simulation speed and number of Greenflies and Ladybirds must be a positive Integer!!");
                        inputsCorrect = false;
                    }

                    numsOfCells = rows * cols;
                    numsOfCreatures = gfs + lbs;

                    cellsLessThanCreatures = numsOfCells < numsOfCreatures;
                    if (cellsLessThanCreatures)
                    {
                        Console.WriteLine("\nError,The number of cells ({0}) is smaller than the Greenflies + Ladybirds ({1})!!", numsOfCells, numsOfCreatures);
                        Console.WriteLine("Renter correct values!!\n");
                    }

                } while (!inputsCorrect || cellsLessThanCreatures);
            }
            
            world = new World((int)rows, (int)cols, (int)gfs, (int)lbs);
            Simulate();
        }

        public void Continue()
        {
            bool redraw = true;
            bool newStart = false;
            do
            {
                ConsoleKeyInfo k = Console.ReadKey();

                if (k.Key == ConsoleKey.S) // When the user press 's' they will be able to change the simulation speed and Display mode.
                {
                    try
                    {
                        Console.Write("\nSimulation Speed: ");
                        SimulationSpeed = Convert.ToUInt32(Console.ReadLine());

                        Console.Write("\nDisplay Mode: ");
                        Mode = Convert.ToUInt32(Console.ReadLine());

                        Simulate();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine(" Please only Use Positive Integers!!!");
                    }

                    if (SimulationSpeed == 0)
                    {
                        Console.WriteLine("\nSimulation speed must be larger than 0!!");
                    }

                    if (Mode != 0 && Mode != 1)
                    {
                        Console.WriteLine("\nDisplay mode must be 0 or 1!!");
                    }

                }

                else if (k.Key == ConsoleKey.Enter)
                {
                    Simulate();
                }

                else if (k.Key == ConsoleKey.Escape)
                {
                    redraw = false;
                    world.WriteToFile(createNewFile);
                    Console.WriteLine("\n You Exited the Game.");
                }

                else if (k.Key == ConsoleKey.N)
                {
                    redraw = false;
                    createNewFile = false;
                    world.WriteToFile(false);
                    newStart = true;
                    
                }

            } while (redraw);

            if (newStart == true)
            {
                Start();
                Continue();
            }
        }

        private void Simulate()
        {
            if (Mode == 0) // The First Display Mode: Every grid will be drawn.
            {
                for (int i = 0; i < SimulationSpeed; i++)
                {
                    world.timeStep++;
                    world.Generate();
                    ConsoleDraw();
                }
            }

            else if (Mode == 1) //The Second Display Mode: Only the last grid will be drawn. 
            {
                for (int i = 0; i < SimulationSpeed; i++)
                {
                    world.timeStep++;
                    world.Generate();
                }
                ConsoleDraw();
            }
        }

        private void ConsoleDraw() // This method handls how the grid and the inforamtion will be dispalyed on the console.
        {
            string line = new string('-', world.Width);
            Console.WriteLine(" " + line);
            world.Draw();
            Console.WriteLine(" " + line);
            int[] count = world.GetTheCounts();
            Console.WriteLine("\nGreenFlies: " + count[0] + "|||" + "Ladybirds: " + count[1] + "|||" + "TimeStep: " + world.timeStep + "|||" + "Speed: " + SimulationSpeed + "|||" + "Mode: " + Mode);
            Console.WriteLine("To change settings press (s), To continue press Enter, To start a new world press (n),otherwise press Escape (ESC) key to end the game.");
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
