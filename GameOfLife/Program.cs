//AYoub Sikouky.
// 15/11/2019.

using System;

namespace GameOfLife
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("This a Simulation of the Game of Life, the main parts of it are: ");
            Console.WriteLine("1.The world which is a grid of cells, each cell can either contain a Ladybird, Greenfly or be empty.");
            Console.WriteLine("2.Ladybirds 'x' ,can eat greenflies or move each turn, if it didn't eat any greenflies in the last 3 turns it dies. it can breed after 8 turns.");
            Console.WriteLine("3.Greenflies 'o', can only moves and breed after 3 turns.\n");
            Console.WriteLine("Make sure that the total number of cell is equal or bigger than the number of greenflies and ladybirds combined!!");
            Console.WriteLine("-----------------------------------------------------------------------------------------------------");
            // Need to check if the inputs are a number and that they can hold all the GFs and LBs.
            uint rows = 0, cols = 0, gfs = 0, lbs = 0, numsOfCells = 0, numsOfCreatures = 0, simulationSpeed = 1;
            bool rule1, rule2;
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

                    rule1 = true;
                }
                catch (Exception)
                {
                    Console.WriteLine("Rows, columns, simulation speed and number of Greenflies and Ladybirds must be a positive Integer!!");
                    rule1 = false;
                }
                

                numsOfCells = rows * cols;
                numsOfCreatures = gfs + lbs;

                rule2 =  numsOfCells < numsOfCreatures;
                if (rule2)
                {
                    Console.WriteLine("The number of cells ({0}) is smaller than the Greenflies + Ladybirds ({1})!!", numsOfCells, numsOfCreatures);
                    Console.WriteLine("Renter correct values!!\n");
                }
            } while (!rule1 || rule2);


            World world = new World((int)rows, (int)cols, (int)gfs, (int)lbs);
            ConsoleDraw(world, simulationSpeed);
            bool redraw = true;

            do
            {
                ConsoleKeyInfo k = Console.ReadKey();

                if (k.Key == ConsoleKey.S)
                {
                    try
                    {
                        Console.Write("\nSimulation Speed: ");
                        simulationSpeed = Convert.ToUInt32(Console.ReadLine());
                        ConsoleDraw(world, simulationSpeed);
                    }
                    catch( Exception)
                    {
                        Console.WriteLine("Please only Use Positive Integers!!!");
                    }
                    
                }

                else if (k.Key == ConsoleKey.Enter)
                {
                    ConsoleDraw(world, simulationSpeed);
                }
                else if(k.Key == ConsoleKey.Escape)
                {
                    redraw = false;
                }

            } while (redraw);
            //world.WriteToFile();
            Console.WriteLine("\n You Exited the Game.");
        }

        public static void ConsoleDraw(World wld, uint speed)
        {
            string line = new string('-', wld.Width);
            for (int i = 0; i < speed; i++)
            {
                wld.timeStep++;
                Console.WriteLine(" " + line);
                wld.Draw();
                Console.WriteLine(" " + line);
                // Temporary !!!!!!!!
                int[] count = wld.checkTheCount();
                Console.WriteLine("\nGreenFlies: " + count[0] + "|||" + "Ladybirds: " + count[1] + "|||" + "TimeStep: " + wld.timeStep + "|||" + "Speed: " + speed);
                Console.WriteLine("To change speed press (s), To continue press Enter, otherwise press Escape (ESC) key to end the game.");
                Console.WriteLine();
                Console.WriteLine();

            }
            
        }
    }
}
