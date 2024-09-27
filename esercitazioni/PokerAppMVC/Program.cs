using PokerAppMVC.Controllers;
using PokerAppMVC.Models;
using PokerAppMVC.Views;
using System;

namespace PokerAppMVC
{
    class Program
    {
        static void Main(string[] args)
        {
            var playerController = new PlayerController();
            var handController = new HandController();
            var playerView = new PlayerView();
            var handView = new HandView();

            Console.WriteLine("Welcome to the Poker App!");
            Console.Write("Enter your name: ");
            string playerName = Console.ReadLine();

            // Create a new player or retrieve an existing player
            var player = playerController.GetPlayer(playerName);
            if (player == null)
            {
                player = playerController.CreatePlayer(playerName);
                Console.WriteLine("New player created.");
            }
            else
            {
                Console.WriteLine("Player loaded from database.");
            }

            playerView.DisplayPlayerInfo(player);

            // Start a new hand and save it to the database
            Console.WriteLine("Starting a new hand...");
            handController.SaveHand(player.PlayerId);

            // Retrieve and display all hands for the player
            var hands = handController.GetHandsByPlayer(player.PlayerId);
            handView.DisplayAllHands(hands);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
