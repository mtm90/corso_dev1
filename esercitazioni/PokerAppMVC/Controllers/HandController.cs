using PokerAppMVC.Models;
using System.Collections.Generic;

namespace PokerAppMVC.Controllers
{
    public class HandController
    {
        private readonly PokerDbContext _context;
        private Deck _deck;
        private List<Card> _playerHand;
        private List<Card> _computerHand;
        private List<Card> _communityCards;

        public HandController()
        {
            _context = new PokerDbContext();
            _deck = new Deck();
            _playerHand = new List<Card>();
            _computerHand = new List<Card>();
            _communityCards = new List<Card>();
        }

        // Reset the deck and hands for a new hand
        public void InitializeNewHand()
        {
            _deck = new Deck();
            _deck.Shuffle();

            _playerHand.Clear();
            _computerHand.Clear();
            _communityCards.Clear();
        }

        // Deal two cards to both the player and computer
        public void DealPreflop()
        {
            _playerHand.Add(_deck.Deal());
            _playerHand.Add(_deck.Deal());

            _computerHand.Add(_deck.Deal());
            _computerHand.Add(_deck.Deal());
        }

        // Deal three community cards for the flop
        public void DealFlop()
        {
            _communityCards.Add(_deck.Deal());
            _communityCards.Add(_deck.Deal());
            _communityCards.Add(_deck.Deal());
        }

        // Deal one card for the turn
        public void DealTurn()
        {
            _communityCards.Add(_deck.Deal());
        }

        // Deal one card for the river
        public void DealRiver()
        {
            _communityCards.Add(_deck.Deal());
        }

        // Get the player's hand as a string
        public string GetPlayerHand()
        {
            return string.Join(" ", _playerHand);
        }

        // Get the computer's hand as a string
        public string GetComputerHand()
        {
            return string.Join(" ", _computerHand);
        }

        // Get the community cards as a string
        public string GetCommunityCards()
        {
            return string.Join(" ", _communityCards);
        }

        // Save the current hand state to the database
        public void SaveCurrentHand(int playerId)
        {
            var playerHand = GetPlayerHand();
            var computerHand = GetComputerHand();
            var playerStack = 500;  // Example value, update as needed
            var computerStack = 500;  // Example value, update as needed

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
    }
}
