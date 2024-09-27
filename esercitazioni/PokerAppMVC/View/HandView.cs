using PokerAppMVC.Models;
using System;

namespace PokerAppMVC.Views
{
    public class HandView
    {
        public void DisplayPreflop(string playerHand, string computerHand)
        {
            Console.WriteLine("Preflop Stage:");
            Console.WriteLine($"Player's Hand: {playerHand}");
            Console.WriteLine($"Computer's Hand: {computerHand}");
        }

        public void DisplayFlop(string communityCards)
        {
            Console.WriteLine("Flop Stage:");
            Console.WriteLine($"Community Cards: {communityCards}");
        }

        public void DisplayTurn(string communityCards)
        {
            Console.WriteLine("Turn Stage:");
            Console.WriteLine($"Community Cards: {communityCards}");
        }

        public void DisplayRiver(string communityCards)
        {
            Console.WriteLine("River Stage:");
            Console.WriteLine($"Community Cards: {communityCards}");
        }
    }
}
