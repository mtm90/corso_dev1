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

            // Post blinds
            handController.PostBlinds();

            // Deal preflop (both players receive cards)
            handController.DealPreflop();

            // Flop Stage
            handController.BettingRound("Preflop");
            handController.DealCardsForStage("Flop");
            handView.DisplayFlop(handController.GetCommunityCards());
            handController.BettingRound("Flop");

            // Turn Stage
            handController.DealCardsForStage("Turn");
            handView.DisplayTurn(handController.GetCommunityCards());
            handController.BettingRound("Turn");

            // River Stage
            handController.DealCardsForStage("River");
            handView.DisplayRiver(handController.GetCommunityCards());
            handController.BettingRound("River");

            // Save the current hand to the database
            handController.SaveCurrentHand(player.PlayerId);

            Console.WriteLine("Hand saved to the database.");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
