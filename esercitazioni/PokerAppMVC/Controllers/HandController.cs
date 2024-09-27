using PokerAppMVC.Models;
using System.Collections.Generic;

namespace PokerAppMVC.Controllers
{
    public class HandController
    {
        private readonly PokerDbContext _context;
        private Deck _deck;

        public HandController()
        {
            _context = new PokerDbContext();
            _deck = new Deck();
        }

        // Save a new hand to the database
        public void SaveHand(int playerId, int numberOfCards = 2)
        {
            _deck.Shuffle(); // Shuffle the deck before dealing
            var playerHand = _deck.Deal(numberOfCards);
            var computerHand = _deck.Deal(numberOfCards);

            var hand = new Hand
            {
                PlayerId = playerId,
                PlayerHand = string.Join(", ", playerHand),
                ComputerHand = string.Join(", ", computerHand),
                PlayerStack = 500, // Example value
                ComputerStack = 500 // Example value
            };

            _context.Hands.Add(hand);
            _context.SaveChanges();
        }

        // Retrieve all hands for a player
        public List<Hand> GetHandsByPlayer(int playerId)
        {
            return _context.Hands.Where(h => h.PlayerId == playerId).ToList();
        }
    }
}
