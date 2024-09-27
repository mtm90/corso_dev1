using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerAppMVC.Models
{
    public class Deck
    {
        private List<Card> Cards { get; set; }
        private Random random = new Random();

        public Deck()
        {
            // Initialize the deck with 52 cards
            string[] suits = { "♠", "♥", "♦", "♣" };
            string[] values = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

            Cards = new List<Card>();

            foreach (var suit in suits)
            {
                foreach (var value in values)
                {
                    Cards.Add(new Card(suit, value));
                }
            }
        }

        // Shuffle the deck
        public void Shuffle()
        {
            Cards = Cards.OrderBy(x => random.Next()).ToList();
        }

        // Deal a single card from the deck
        public Card Deal()
        {
            if (Cards.Count > 0)
            {
                var card = Cards[0];
                Cards.RemoveAt(0);
                return card;
            }
            throw new InvalidOperationException("No cards left in the deck");
        }
    }
}
