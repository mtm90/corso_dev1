using PokerAppMVC.Controllers;
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
            var handView = new HandView();

            Console.WriteLine("Welcome to the Poker App!");

            // Prompt for player's name and create/retrieve player
            Console.Write("Enter your name: ");
            string playerName = Console.ReadLine();

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

            // Start a new hand
            handController.InitializeNewHand();

            // Deal preflop (no community cards shown)
            handController.DealPreflop();
            handView.DisplayPreflop(handController.GetPlayerHand(), handController.GetComputerHand());

            // Deal flop (3 community cards)
            handController.DealFlop();
            handView.DisplayFlop(handController.GetCommunityCards());

            // Deal turn (1 more community card)
            handController.DealTurn();
            handView.DisplayTurn(handController.GetCommunityCards());

            // Deal river (1 more community card)
            handController.DealRiver();
            handView.DisplayRiver(handController.GetCommunityCards());

            // Save the current hand to the database
            handController.SaveCurrentHand(player.PlayerId);

            Console.WriteLine("Hand saved to the database.");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
