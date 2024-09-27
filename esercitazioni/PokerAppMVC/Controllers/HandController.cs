using PokerAppMVC.Models;
using System.Collections.Generic;
using System.Linq;

namespace PokerAppMVC.Controllers
{
    public class HandController
    {
        private readonly PokerDbContext _context;

        public HandController()
        {
            _context = new PokerDbContext();
        }

        // Save a new hand to the database
        public void SaveHand(int playerId, string playerHand, string computerHand, int playerStack, int computerStack)
        {
            var hand = new Hand
            {
                PlayerId = playerId,
                PlayerHand = playerHand,
                ComputerHand = computerHand,
                PlayerStack = playerStack,
                ComputerStack = computerStack
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
