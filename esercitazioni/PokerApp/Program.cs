using PokerApp.Controllers;
using PokerApp.Models;
using System;

namespace PokerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var controller = new GameController();
            Console.WriteLine("Welcome to Poker!");
            Console.WriteLine("1. Start New Game");
            Console.WriteLine("2. Load Game");
            var choice = Console.ReadLine();

            if (choice == "1")
            {
                controller.StartNewGame();
                Console.WriteLine("New game started!");
            }
            else if (choice == "2")
            {
                Console.WriteLine("Enter Game ID:");
                var gameId = int.Parse(Console.ReadLine());
                var game = controller.LoadGame(gameId);
                if (game != null)
                {
                    Console.WriteLine("Game loaded!");
                    // Display game state
                }
                else
                {
                    Console.WriteLine("Game not found.");
                }
            }
        }
    }
}