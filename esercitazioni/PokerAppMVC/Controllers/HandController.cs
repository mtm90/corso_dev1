using PokerAppMVC.Models;
using System.Collections.Generic;
using System;

namespace PokerAppMVC.Controllers
{
    public class HandController
    {
        private readonly PokerDbContext _context;
        private Deck _deck;
        private List<Card> _playerHand;
        private List<Card> _computerHand;
        private List<Card> _communityCards;

        private int _playerStack;
        private int _computerStack;
        private int _pot;
        private int _playerBet;     // Define player bet field
        private int _computerBet;   // Define computer bet field
        private bool _isPlayerSmallBlind;
        private int _smallBlind = 1;
        private int _bigBlind = 2;

        public HandController()
        {
            _context = new PokerDbContext();
            _deck = new Deck();
            _playerHand = new List<Card>();
            _computerHand = new List<Card>();
            _communityCards = new List<Card>();

            // Initialize stacks and blinds
            _playerStack = 500;
            _computerStack = 500;
            _pot = 0;
            _playerBet = 0;         // Initialize player bet
            _computerBet = 0;       // Initialize computer bet
            _isPlayerSmallBlind = true; // Player starts as small blind
        }

        // Initialize a new hand and shuffle the deck
        public void InitializeNewHand()
        {
            _deck = new Deck();
            _deck.Shuffle();

            _playerHand.Clear();
            _computerHand.Clear();
            _communityCards.Clear();
            _pot = 0;
            _playerBet = 0;
            _computerBet = 0;
        }

        // Deal two cards to both the player and computer
        public void DealPreflop()
        {
            _playerHand.Add(_deck.Deal());
            _playerHand.Add(_deck.Deal());

            _computerHand.Add(_deck.Deal());
            _computerHand.Add(_deck.Deal());

            Console.WriteLine($"Player's hand: {GetPlayerHand()}");
            Console.WriteLine($"Computer's hand: {GetComputerHand()} (hidden)");
        }

        // Post the blinds for the current hand
        public void PostBlinds()
        {
            if (_isPlayerSmallBlind)
            {
                _playerStack -= _smallBlind;
                _computerStack -= _bigBlind;
                _playerBet = _smallBlind;
                _computerBet = _bigBlind;
                _pot += _smallBlind + _bigBlind;
                Console.WriteLine($"Player posts small blind of {_smallBlind}");
                Console.WriteLine($"Computer posts big blind of {_bigBlind}");
            }
            else
            {
                _computerStack -= _smallBlind;
                _playerStack -= _bigBlind;
                _computerBet = _smallBlind;
                _playerBet = _bigBlind;
                _pot += _smallBlind + _bigBlind;
                Console.WriteLine($"Computer posts small blind of {_smallBlind}");
                Console.WriteLine($"Player posts big blind of {_bigBlind}");
            }
        }

        // Manage a betting round for the given stage
        // Modify the BettingRound method to ensure both player and computer take actions properly
public void BettingRound(string stage)
{
    Console.WriteLine($"--- {stage} Betting Round ---");
    bool playerTurn = _isPlayerSmallBlind ? false : true; // Determine who starts based on blinds
    bool bettingComplete = false;  // Flag to indicate if betting round is complete

    // Reset player and computer bets to 0 for a new round
    _playerBet = 0;
    _computerBet = 0;

    // Keep looping until betting round is completed
    while (!bettingComplete)
    {
        if (playerTurn)
        {
            PlayerAction(); // Handle player's betting action
        }
        else
        {
            ComputerAction(); // Handle computer's betting action
        }

        // Check if both player and computer have taken an action and if betting is complete (both bets are equal)
        if (_playerBet == _computerBet && (_playerBet > 0 || _computerBet > 0))
        {
            bettingComplete = true;
            Console.WriteLine("Betting round complete.");
        }
        else
        {
            playerTurn = !playerTurn; // Switch turns if bets are not equal or no actions taken yet
        }
    }
}


public void PlayerAction()
{
    Console.WriteLine($"Player Stack: {_playerStack}, Computer Stack: {_computerStack}, Pot: {_pot}");
    Console.WriteLine("Choose your action: (1) Bet/Raise (2) Call (3) Fold");
    string input = Console.ReadLine();

    switch (input)
    {
        case "1": // Bet/Raise
            Console.WriteLine("Enter the amount to bet/raise:");
            int raiseAmount;
            if (!int.TryParse(Console.ReadLine(), out raiseAmount) || raiseAmount <= 0)
            {
                Console.WriteLine("Invalid bet amount. Please enter a positive integer.");
                PlayerAction(); // Retry on invalid input
                return;
            }
            if (raiseAmount > _playerStack)
            {
                Console.WriteLine("You cannot bet more than your stack.");
                PlayerAction(); // Retry if bet is too large
                return;
            }
            _playerBet += raiseAmount;
            _playerStack -= raiseAmount;
            _pot += raiseAmount;
            Console.WriteLine($"Player raises by {raiseAmount}. Player bet: {_playerBet}");
            break;

        case "2": // Call
            int callAmount = _computerBet - _playerBet;
            if (callAmount > _playerStack)
            {
                callAmount = _playerStack; // Player goes all-in if call amount is more than stack
            }
            _playerBet += callAmount;
            _playerStack -= callAmount;
            _pot += callAmount;
            Console.WriteLine($"Player calls {callAmount}. Player bet: {_playerBet}");
            break;

        case "3": // Fold
            Console.WriteLine("Player folds. Computer wins the pot.");
            _computerStack += _pot;
            _pot = 0;
            return; // End the current betting round
        default:
            Console.WriteLine("Invalid action. Try again.");
            PlayerAction(); // Retry on invalid action
            break;
    }
}

public void ComputerAction()
{
    // Calculate the amount the computer needs to call
    int callAmount = _playerBet - _computerBet;

    // Ensure the call amount is not negative
    if (callAmount < 0)
    {
        callAmount = 0; // Reset call amount to 0 if negative
    }

    // If the computer has enough stack to call the amount
    if (callAmount <= _computerStack)
    {
        // Computer calls the amount and updates its bet and stack
        _computerBet += callAmount;
        _computerStack -= callAmount;
        _pot += callAmount;
        Console.WriteLine($"Computer calls {callAmount}. Computer bet: {_computerBet}");
    }
    else
    {
        // If the computer doesn't have enough to call, it folds
        Console.WriteLine("Computer folds. Player wins the pot.");
        _playerStack += _pot; // Award the pot to the player
        _pot = 0; // Reset the pot to 0
    }
}


        // Deal cards for different stages
        public void DealCardsForStage(string stage)
        {
            switch (stage)
            {
                case "Flop":
                    DealFlop();
                    break;
                case "Turn":
                    DealTurn();
                    break;
                case "River":
                    DealRiver();
                    break;
            }
        }

        // Deal flop (3 cards)
        public void DealFlop()
        {
            _communityCards.Add(_deck.Deal());
            _communityCards.Add(_deck.Deal());
            _communityCards.Add(_deck.Deal());
        }

        // Deal turn (1 card)
        public void DealTurn()
        {
            _communityCards.Add(_deck.Deal());
        }

        // Deal river (1 card)
        public void DealRiver()
        {
            _communityCards.Add(_deck.Deal());
        }

        // Save the current hand state to the database
        public void SaveCurrentHand(int playerId)
        {
            var playerHand = GetPlayerHand();
            var computerHand = GetComputerHand();

            var hand = new Hand
            {
                PlayerId = playerId,
                PlayerHand = playerHand,
                ComputerHand = computerHand,
                PlayerStack = _playerStack,
                ComputerStack = _computerStack,
                Pot = _pot,
                PlayerBet = _playerBet,
                ComputerBet = _computerBet
            };
            _context.Hands.Add(hand);
            _context.SaveChanges();
        }

        public string GetPlayerHand()
        {
            return string.Join(" ", _playerHand);
        }

        public string GetComputerHand()
        {
            return string.Join(" ", _computerHand);
        }

        public string GetCommunityCards()
        {
            return string.Join(" ", _communityCards);
        }
    }
}
