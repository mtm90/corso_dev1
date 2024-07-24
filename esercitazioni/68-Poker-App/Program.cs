﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static string[] deck = new string[52];
    static string[] playerHand = new string[2];
    static string[] computerHand = new string[2];
    static string[] communityCards = new string[5];
    static int currentCardIndex = 0;
    static int playerStack = 200;
    static int computerStack = 200;
    static int smallBlind = 1;
    static int bigBlind = 2;
    static int pot = 0;
    static int playerBet = 0;
    static int computerBet = 0;
    static bool isPlayerSmallBlind = true;
    static string gameFilePath = "game_data.json";

    static void Main()
    {
        int input;
        do
        {
            Console.Clear();
            Console.WriteLine("Welcome to The Poker App! Please choose one of the following options:");
            Console.WriteLine("1. Start new game");
            Console.WriteLine("2. Load game");
            Console.WriteLine("3. Erase game");
            Console.WriteLine("4. View games");
            Console.WriteLine("5. Quit");
            input = Convert.ToInt32(Console.ReadLine());

            switch (input)
            {
                case 1:
                    StartNewGame(false);
                    break;
                case 2:
                    LoadGame();
                    break;
                case 3:
                    EraseGame();
                    break;
                case 4:
                    ViewGameHistory();
                    break;
                case 5:
                    Console.WriteLine("Quitting...");
                    break;
                default:
                    Console.WriteLine("Invalid option, please try again");
                    WaitForUserInput();
                    break;
            }
        } while (input != 5);
    }

    static void StartNewGame(bool isLoadedGame)
    {
        if (!isLoadedGame)
        {
            playerStack = 200;
            computerStack = 200;
        }

        while (playerStack > 0 && computerStack > 0)
        {
            Console.Clear();

            if (!isLoadedGame)
            {
                InitializeDeck();
                ShuffleDeck();
                currentCardIndex = 0;
                pot = 0;
                playerBet = 0;
                computerBet = 0;
            }

            DealInitialCards();
            HandleBlinds();
            Console.Clear();
            Console.WriteLine("Preflop: No community cards yet.");
            DisplayPlayerHand();
            DisplayPot();
            DisplayStacks();

            bool handEnded = false;
            if (isPlayerSmallBlind)
            {
                handEnded = !BettingRound(true, true);
            }
            else
            {
                handEnded = !BettingRound(false, true);
            }

            if (handEnded)
            {
                EndHand();
                continue;
            }

            Console.WriteLine("Flop:");
            DealCommunityCards(1);
            DisplayCommunityCards();
            DisplayPot();
            DisplayStacks();
            if (!isPlayerSmallBlind)
            {
                handEnded = !BettingRound(true, false);
            }
            else
            {
                handEnded = !BettingRound(false, false);
            }

            if (handEnded)
            {
                EndHand();
                continue;
            }

            Console.WriteLine("Turn:");
            DealCommunityCards(2);
            DisplayCommunityCards();
            DisplayPot();
            DisplayStacks();
            if (!isPlayerSmallBlind)
            {
                handEnded = !BettingRound(true, false);
            }
            else
            {
                handEnded = !BettingRound(false, false);
            }

            if (handEnded)
            {
                EndHand();
                continue;
            }

            Console.WriteLine("River:");
            DealCommunityCards(3);
            DisplayCommunityCards();
            DisplayPot();
            DisplayStacks();
            if (!isPlayerSmallBlind)
            {
                handEnded = !BettingRound(true, false);
            }
            else
            {
                handEnded = !BettingRound(false, false);
            }

            if (handEnded)
            {
                EndHand();
                continue;
            }

            Console.WriteLine($"Computer hand: {computerHand[0]} {computerHand[1]}");
            DetermineWinner();
            EndHand();

            isLoadedGame = false; // Reset after the first loop
        }

        Console.WriteLine("Game Over");
        Console.WriteLine($"Final Player Stack: {playerStack}");
        Console.WriteLine($"Final Computer Stack: {computerStack}");
        WaitForUserInput();
    }

    static void EndHand()
    {
        Console.WriteLine("Hand Over");
        Console.WriteLine($"Pot: {pot}");
        Console.WriteLine($"Player Stack: {playerStack}");
        Console.WriteLine($"Computer Stack: {computerStack}");
        Array.Clear(communityCards, 0, communityCards.Length);
        playerBet = 0;
        computerBet = 0;
        pot = 0;
        isPlayerSmallBlind = !isPlayerSmallBlind;
        SaveGame();
        WaitForUserInput();
    }

    static void InitializeDeck()
    {
        string[] suits = { "\u2664", "\u2661", "\u2662", "\u2667" };
        string[] values = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
        int index = 0;
        for (int i = 0; i < suits.Length; i++)
        {
            for (int j = 0; j < values.Length; j++)
            {
                deck[index] = $"{values[j]}{suits[i]}";
                index++;
            }
        }
    }

    static void ShuffleDeck()
    {
        Random rand = new Random();
        for (int i = deck.Length - 1; i > 0; i--)
        {
            int j = rand.Next(0, i + 1);
            string temp = deck[i];
            deck[i] = deck[j];
            deck[j] = temp;
        }
    }

    static string DealCard()
    {
        return deck[currentCardIndex++];
    }

    static void DealInitialCards()
    {
        playerHand[0] = DealCard();
        playerHand[1] = DealCard();
        computerHand[0] = DealCard();
        computerHand[1] = DealCard();
    }

    static void DealCommunityCards(int stage)
    {
        switch (stage)
        {
            case 1:
                communityCards[0] = DealCard();
                communityCards[1] = DealCard();
                communityCards[2] = DealCard();
                break;
            case 2:
                communityCards[3] = DealCard();
                break;
            case 3:
                communityCards[4] = DealCard();
                break;
        }
    }

    static void HandleBlinds()
    {
        if (isPlayerSmallBlind)
        {
            playerStack -= smallBlind;
            playerBet = smallBlind;
            computerStack -= bigBlind;
            computerBet = bigBlind;
            pot += smallBlind + bigBlind;
            Console.WriteLine($"Player posts small blind of {smallBlind}");
            Console.WriteLine($"Computer posts big blind of {bigBlind}");
        }
        else
        {
            playerStack -= bigBlind;
            playerBet = bigBlind;
            computerStack -= smallBlind;
            computerBet = smallBlind;
            pot += bigBlind + smallBlind;
            Console.WriteLine($"Computer posts small blind of {smallBlind}");
            Console.WriteLine($"Player posts big blind of {bigBlind}");
        }
    }

static bool BettingRound(bool playerStarts, bool isPreflop)
{
    bool roundOver = false;

    while (!roundOver)
    {
        if (playerStarts)
        {
            if (!PlayerAction(isPreflop)) return false; // Player action
            if (playerBet == computerBet) // Check or Call leads to the computer's turn
            {
                if (!ComputerAction()) return false; // Computer action
                if (playerBet == computerBet) break; // Round over if bets are equal
            }
        }
        else
        {
            if (!ComputerAction()) return false; // Computer action
            if (playerBet == computerBet) // Check or Call leads to the player's turn
            {
                if (!PlayerAction(isPreflop)) return false; // Player action
                if (playerBet == computerBet) break; // Round over if bets are equal
            }
        }

        // If the bets are not equal, continue the round
        if (playerBet != computerBet)
        {
            if (playerStarts)
            {
                if (!ComputerAction()) return false; // Computer action
                if (playerBet == computerBet) break; // Round over if bets are equal
            }
            else
            {
                if (!PlayerAction(isPreflop)) return false; // Player action
                if (playerBet == computerBet) break; // Round over if bets are equal
            }
        }
    }

    return true;
}
    static bool PlayerAction(bool isPreflop)
{
    Console.WriteLine("Enter your action: (1) Bet/Raise (2) Call (3) Check (4) Fold");
    int action = Convert.ToInt32(Console.ReadLine());
    int callAmount = computerBet - playerBet;

    switch (action)
    {
        case 1: // Bet/Raise
            Console.WriteLine("Enter your bet/raise amount:");
            int amount = Convert.ToInt32(Console.ReadLine());
            if (amount < callAmount)
            {
                Console.WriteLine($"You must at least call {callAmount}.");
                return PlayerAction(isPreflop); // Retry if the raise amount is not valid
            }
            if (amount > playerStack)
            {
                amount = playerStack; // Bet only the remaining stack if not enough
            }
            playerStack -= amount;
            pot += amount;
            playerBet += amount;
            Console.WriteLine($"You bet/raised {amount}");
            break;

        case 2: // Call
            if (callAmount > playerStack)
            {
                callAmount = playerStack; // Call only the remaining stack if not enough
            }
            playerStack -= callAmount;
            pot += callAmount;
            playerBet = computerBet;
            Console.WriteLine($"You call {callAmount}");
            break;

        case 3: // Check
            if (playerBet != computerBet)
            {
                Console.WriteLine($"You can only check if the current bet is {computerBet}.");
                return PlayerAction(isPreflop); // Retry if check is not valid
            }
            Console.WriteLine("You check.");
            break;

        case 4: // Fold
            Console.WriteLine("You folded.");
            computerStack += pot; // Award the pot to the computer
            return false; // Return false to indicate the player has folded

        default:
            Console.WriteLine("Invalid action. Try again.");
            return PlayerAction(isPreflop); // Retry if the action is invalid
    }
    return true; // Return true if the action was valid
}

static bool ComputerAction()
{
    Random rand = new Random();
    int action = rand.Next(1, 5); // Randomly choose between Bet/Raise, Call, Check, Fold
    int callAmount = playerBet - computerBet;

    switch (action)
    {
        case 1: // Bet/Raise
            if (computerStack == 0)
            {
                action = 3; // Change action to Check if the stack is 0
            }
            else
            {
                int amount = rand.Next(bigBlind, pot); // Ensure the bet is within the stack limit
                computerStack -= amount;
                pot += amount;
                computerBet += amount;
                Console.WriteLine($"Computer bets/raises {amount}");
            }
            break;

        case 2: // Call
            if (callAmount > computerStack)
            {
                callAmount = computerStack; // Call only the remaining stack if not enough
            }
            computerStack -= callAmount;
            pot += callAmount;
            computerBet = playerBet;
            Console.WriteLine($"Computer calls {callAmount}");
            break;

        case 3: // Check
            if (computerBet != playerBet)
            {
                return ComputerAction(); // Retry if check is not valid
            }
            Console.WriteLine("Computer checks.");
            break;

        case 4: // Fold
            Console.WriteLine("Computer folds.");
            playerStack += pot; // Award the pot to the player
            return false; // Return false to indicate the computer has folded

        default:
            Console.WriteLine("Computer takes an invalid action.");
            return ComputerAction(); // Retry if action is invalid
    }
    return true; // Return true if the action was valid
}

    static void DetermineWinner()
    {
        int playerScore = EvaluateHand(playerHand, communityCards, out int playerHighCard);
        int computerScore = EvaluateHand(computerHand, communityCards, out int computerHighCard);

        if (playerScore > computerScore || (playerScore == computerScore && playerHighCard > computerHighCard))
        {
            Console.WriteLine("Player wins the hand.");
            playerStack += pot;
        }
        else if (computerScore > playerScore || (playerScore == computerScore && computerHighCard > playerHighCard))
        {
            Console.WriteLine("Computer wins the hand.");
            computerStack += pot;
        }
        else
        {
            Console.WriteLine("It's a tie!");
            playerStack += pot / 2;
            computerStack += pot / 2;
        }
    }

    static int EvaluateHand(string[] hand, string[] communityCards, out int highCard)
    {
        List<string> allCards = new List<string>(hand);
        allCards.AddRange(communityCards);
        string[] cardValues = new string[allCards.Count];
        char[] cardSuits = new char[allCards.Count];

        for (int i = 0; i < allCards.Count; i++)
        {
            string card = allCards[i];
            cardValues[i] = card.Length == 3 ? card.Substring(0, 2) : card[0].ToString();
            cardSuits[i] = card.Last();
        }

        Array.Sort(cardValues, (a, b) => GetCardValue(b).CompareTo(GetCardValue(a)));
        Array.Sort(cardSuits);

        if (IsRoyalFlush(cardValues, cardSuits)) { highCard = GetCardValue(cardValues[0]); return 10; }
        if (IsStraightFlush(cardValues, cardSuits)) { highCard = GetCardValue(cardValues[0]); return 9; }
        if (IsFourOfAKind(cardValues)) { highCard = GetCardValue(cardValues[0]); return 8; }
        if (IsFullHouse(cardValues)) { highCard = GetCardValue(cardValues[0]); return 7; }
        if (IsFlush(cardSuits)) { highCard = GetCardValue(cardValues[0]); return 6; }
        if (IsStraight(cardValues)) { highCard = GetCardValue(cardValues[0]); return 5; }
        if (IsThreeOfAKind(cardValues)) { highCard = GetCardValue(cardValues[0]); return 4; }
        if (IsTwoPair(cardValues, out highCard)) { return 3; }
        if (IsOnePair(cardValues, out highCard)) { return 2; }

        highCard = GetCardValue(cardValues[0]);
        return 1;
    }

    static int GetCardValue(string card)
    {
        switch (card)
        {
            case "2": return 2;
            case "3": return 3;
            case "4": return 4;
            case "5": return 5;
            case "6": return 6;
            case "7": return 7;
            case "8": return 8;
            case "9": return 9;
            case "10": return 10;
            case "J": return 11;
            case "Q": return 12;
            case "K": return 13;
            case "A": return 14;
            default: throw new Exception("Invalid card value");
        }
    }

    static bool IsRoyalFlush(string[] cardValues, char[] cardSuits)
    {
        return IsFlush(cardSuits) && IsStraight(cardValues) && cardValues.Contains("A");
    }

    static bool IsStraightFlush(string[] cardValues, char[] cardSuits)
    {
        return IsFlush(cardSuits) && IsStraight(cardValues);
    }

    static bool IsFourOfAKind(string[] cardValues)
    {
        for (int i = 0; i < cardValues.Length - 3; i++)
        {
            if (cardValues[i] == cardValues[i + 1] && cardValues[i] == cardValues[i + 2] && cardValues[i] == cardValues[i + 3])
            {
                return true;
            }
        }
        return false;
    }

    static bool IsFullHouse(string[] cardValues)
    {
        bool threeOfAKind = false;
        bool pair = false;
        for (int i = 0; i < cardValues.Length - 2; i++)
        {
            if (cardValues[i] == cardValues[i + 1] && cardValues[i] == cardValues[i + 2])
            {
                threeOfAKind = true;
                break;
            }
        }
        for (int i = 0; i < cardValues.Length - 1; i++)
        {
            if (cardValues[i] == cardValues[i + 1])
            {
                pair = true;
                break;
            }
        }
        return threeOfAKind && pair;
    }

    static bool IsFlush(char[] cardSuits)
    {
        for (int i = 0; i < cardSuits.Length - 4; i++)
        {
            if (cardSuits[i] == cardSuits[i + 1] && cardSuits[i] == cardSuits[i + 2] && cardSuits[i] == cardSuits[i + 3] && cardSuits[i] == cardSuits[i + 4])
            {
                return true;
            }
        }
        return false;
    }

    static bool IsStraight(string[] cardValues)
    {
        int[] cardRanks = cardValues.Select(GetCardValue).ToArray();
        Array.Sort(cardRanks);
        cardRanks = cardRanks.Distinct().ToArray();
        for (int i = 0; i < cardRanks.Length - 4; i++)
        {
            if (cardRanks[i + 4] - cardRanks[i] == 4)
            {
                return true;
            }
        }
        return false;
    }

    static bool IsThreeOfAKind(string[] cardValues)
    {
        for (int i = 0; i < cardValues.Length - 2; i++)
        {
            if (cardValues[i] == cardValues[i + 1] && cardValues[i] == cardValues[i + 2])
            {
                return true;
            }
        }
        return false;
    }

    static bool IsTwoPair(string[] cardValues, out int highCard)
    {
        List<int> pairs = new List<int>();
        for (int i = 0; i < cardValues.Length - 1; i++)
        {
            if (cardValues[i] == cardValues[i + 1])
            {
                pairs.Add(GetCardValue(cardValues[i]));
                i++;
            }
        }
        if (pairs.Count >= 2)
        {
            pairs.Sort();
            highCard = pairs.Last();
            return true;
        }
        highCard = 0;
        return false;
    }

    static bool IsOnePair(string[] cardValues, out int highCard)
    {
        for (int i = 0; i < cardValues.Length - 1; i++)
        {
            if (cardValues[i] == cardValues[i + 1])
            {
                highCard = GetCardValue(cardValues[i]);
                return true;
            }
        }
        highCard = 0;
        return false;
    }

    static void DisplayPlayerHand()
    {
        Console.WriteLine($"Your hand: {playerHand[0]} {playerHand[1]}");
    }

    static void DisplayCommunityCards()
    {
        Console.WriteLine("Community cards: " + string.Join(" ", communityCards.Where(card => card != null)));
    }

    static void DisplayPot()
    {
        Console.WriteLine($"Pot: {pot}");
    }

    static void DisplayStacks()
    {
        Console.WriteLine($"Your stack: {playerStack}");
        Console.WriteLine($"Computer stack: {computerStack}");
    }

    static void WaitForUserInput()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    static void EraseGame()
    {
        if (File.Exists(gameFilePath))
        {
            File.Delete(gameFilePath);
            Console.WriteLine("Game data erased.");
        }
        else
        {
            Console.WriteLine("No game data found to erase.");
        }
        WaitForUserInput();
    }

    static void ViewGameHistory()
{
    if (File.Exists(gameFilePath))
    {
        string json = File.ReadAllText(gameFilePath);
        GameState gameState = JsonConvert.DeserializeObject<GameState>(json);

        Console.WriteLine("Game History:");
        Console.WriteLine($"Player Stack: {gameState.PlayerStack}");
        Console.WriteLine($"Computer Stack: {gameState.ComputerStack}");
        Console.WriteLine($"Pot: {gameState.Pot}");
        Console.WriteLine($"Player Bet: {gameState.PlayerBet}");
        Console.WriteLine($"Computer Bet: {gameState.ComputerBet}");
        Console.WriteLine($"Player Hand: {string.Join(" ", gameState.PlayerHand)}");
        Console.WriteLine($"Computer Hand: {string.Join(" ", gameState.ComputerHand)}");
        Console.WriteLine($"Community Cards: {string.Join(" ", gameState.CommunityCards)}");
        Console.WriteLine($"Current Card Index: {gameState.CurrentCardIndex}");
        Console.WriteLine($"Is Player Small Blind: {gameState.IsPlayerSmallBlind}");
    }
    else
    {
        Console.WriteLine("No saved game found.");
    }
    WaitForUserInput();
}


    static void SaveGame()
    {
        GameState gameState = new GameState
        {
            Deck = deck,
            PlayerHand = playerHand,
            ComputerHand = computerHand,
            CommunityCards = communityCards,
            CurrentCardIndex = currentCardIndex,
            PlayerStack = playerStack,
            ComputerStack = computerStack,
            Pot = pot,
            PlayerBet = playerBet,
            ComputerBet = computerBet,
            IsPlayerSmallBlind = isPlayerSmallBlind
        };

        string json = JsonConvert.SerializeObject(gameState, Formatting.Indented);
        File.WriteAllText(gameFilePath, json);
    }

    static void LoadGame()
    {
        if (File.Exists(gameFilePath))
        {
            string json = File.ReadAllText(gameFilePath);
            GameState gameState = JsonConvert.DeserializeObject<GameState>(json);

            deck = gameState.Deck;
            playerHand = gameState.PlayerHand;
            computerHand = gameState.ComputerHand;
            communityCards = gameState.CommunityCards;
            currentCardIndex = gameState.CurrentCardIndex;
            playerStack = gameState.PlayerStack;
            computerStack = gameState.ComputerStack;
            pot = gameState.Pot;
            playerBet = gameState.PlayerBet;
            computerBet = gameState.ComputerBet;
            isPlayerSmallBlind = gameState.IsPlayerSmallBlind;

            Console.WriteLine("Game loaded successfully.");
            WaitForUserInput();
            StartNewGame(true);
        }
        else
        {
            Console.WriteLine("No saved game found.");
            WaitForUserInput();
        }
    }

    class GameState
    {
        public string[] Deck { get; set; }
        public string[] PlayerHand { get; set; }
        public string[] ComputerHand { get; set; }
        public string[] CommunityCards { get; set; }
        public int CurrentCardIndex { get; set; }
        public int PlayerStack { get; set; }
        public int ComputerStack { get; set; }
        public int Pot { get; set; }
        public int PlayerBet { get; set; }
        public int ComputerBet { get; set; }
        public bool IsPlayerSmallBlind { get; set; }
    }
}
