using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace GameOfLife
{
    static class Program
    {
        public static void Main()
        {
            GameOfLife game = new GameOfLife(25, 25);

            Console.CancelKeyPress += (sender, args) =>
            {
                game.Stop = true;
                Console.WriteLine("\nStop simulation.");
            }; 
            game.Start();
        }
    }
}