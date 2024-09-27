using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerAppMVC.Models
{
    public class Deck
    {
        private List<Card> _cards;
        private Random _random;

        public Deck()
        {
            _random = new Random();
            _cards = new List<Card>();

            string[] suits = { "♠", "♥", "♦", "♣" };
            string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

            foreach (var suit in suits)
            {
                foreach (var rank in ranks)
                {
                    _cards.Add(new Card(suit, rank));
                }
            }
        }

        public void Shuffle()
        {
            _cards = _cards.OrderBy(c => _random.Next()).ToList();
        }

        public List<Card> Deal(int numberOfCards)
        {
            if (numberOfCards > _cards.Count)
                throw new InvalidOperationException("Not enough cards left in the deck.");

            var dealtCards = _cards.Take(numberOfCards).ToList();
            _cards = _cards.Skip(numberOfCards).ToList(); // Remove dealt cards from the deck
            return dealtCards;
        }

        public int CardsRemaining => _cards.Count;
    }
}
