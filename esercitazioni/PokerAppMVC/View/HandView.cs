using PokerAppMVC.Models;
using System;
using System.Collections.Generic;

namespace PokerAppMVC.Views
{
    public class HandView
    {
        public void DisplayHandInfo(Hand hand)
        {
            Console.WriteLine($"Hand ID: {hand.HandId}, Player Hand: {hand.PlayerHand}, Computer Hand: {hand.ComputerHand}, Player Stack: {hand.PlayerStack}, Computer Stack: {hand.ComputerStack}");
        }

        public void DisplayAllHands(List<Hand> hands)
        {
            foreach (var hand in hands)
            {
                DisplayHandInfo(hand);
            }
        }
    }
}
