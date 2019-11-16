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
            Console.WriteLine("1.The world which is a 20x20 grid (400 cells), each cell can either be containing Ladybird, Greenfly or empty.");
            Console.WriteLine("2.Ladybirds 'x' ,can eat greenflies or move each turn, if it didn't eat any greenflies in the last 3 turns it dies. it can breed after 8 turns.");
            Console.WriteLine("3.Greenflies 'o', can only moves and breed after 3 turns.\n");
            Console.WriteLine("Make sure that the total number of cell is equal or bigger than the number of greenflies and ladybirds combined!!");
            Console.WriteLine("-----------------------------------------------------------------------------------------------------");
            // Need to check if the inputs are a number and that they can hold all the GFs and LBs.
            int rows, cols, gfs, lbs, numsOfCells, numsOfCreatures;
            bool rule;
            do
            {
                Console.Title = "Game Of Life";
                Console.Write("Enter number of Rows: ");
                rows = Convert.ToInt32(Console.ReadLine());
                Console.Write("Enter number of Columns: ");
                cols = Convert.ToInt32(Console.ReadLine());
                Console.Write("Enter number of GreenFlies: ");
                gfs = Convert.ToInt32(Console.ReadLine());
                Console.Write("Enter number of LadyBirds: ");

                lbs = Convert.ToInt32(Console.ReadLine());

                numsOfCells = rows * cols;
                numsOfCreatures = gfs + lbs;
                rule =  numsOfCells < numsOfCreatures;
                if (rule)
                {
                    Console.WriteLine("The number of cells ({0}) is smaller than the Greenflies + Ladybirds ({1})!!", numsOfCells, numsOfCreatures);
                    Console.WriteLine("Renter correct values!!\n");
                }
            } while (rule);


            World world = new World(rows, cols, gfs, lbs);
            ConsoleDraw(world);
            bool redraw = true;

            do
            {
                ConsoleKeyInfo k = Console.ReadKey();

                if (k.Key == ConsoleKey.Enter)
                {
                    world.timeStep++;
                    ConsoleDraw(world);
                    Console.WriteLine();
                    Console.WriteLine();
                }
                else
                {
                    redraw = false;
                }

            } while (redraw);
            //world.WriteToFile();
            Console.WriteLine("\nYou Exited the Game.");
        }

        public static void ConsoleDraw(World wld)
        {
            Console.WriteLine(" --------------------");
            wld.Draw();
            Console.WriteLine(" --------------------");
            // Temporary !!!!!!!!
            int[] count = new int[2];
            count = wld.checkTheCount();
            Console.WriteLine("\nGreenFlies: " + count[0] + "|||" + "Ladybirds: " + count[1] + "|||" + "TimeStep: " + wld.timeStep);
            Console.WriteLine("To continue press Enter, otherwise press any key to end the game.");
        }
    }
}
