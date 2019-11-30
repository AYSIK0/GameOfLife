//Ayoub Sikouky.
//Rev.Date: 30/11/2019.

using System;

namespace GameOfLife
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
            game.Continue();
        }
    }
}
