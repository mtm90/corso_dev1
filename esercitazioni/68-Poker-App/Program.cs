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
                    StartNewGame();
                    break;
                case 2:
                    LoadGame();
                    break;
                case 3:
                    EraseGame();
                    break;
                case 4:
                    ViewGames();
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

    static void StartNewGame()
    {
        while (playerStack > 0 && computerStack > 0)
        {
            InitializeDeck();
            ShuffleDeck();
            currentCardIndex = 0;
            pot = 0;
            playerBet = 0;
            computerBet = 0;

            DealInitialCards();
            HandleBlinds();
            Console.WriteLine("Preflop: No community cards yet.");
            DisplayPlayerHand();
            DisplayPot();

            bool handEnded = false;

            if (isPlayerSmallBlind)
            {
                handEnded = !BettingRound(true);
            }
            else
            {
                handEnded = !BettingRound(false);
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

            handEnded = !BettingRound(isPlayerSmallBlind);

            if (handEnded)
            {
                EndHand();
                continue;
            }

            Console.WriteLine("Turn:");
            DealCommunityCards(2);
            DisplayCommunityCards();
            DisplayPot();

            handEnded = !BettingRound(isPlayerSmallBlind);

            if (handEnded)
            {
                EndHand();
                continue;
            }

            Console.WriteLine("River:");
            DealCommunityCards(3);
            DisplayCommunityCards();
            DisplayPot();

            handEnded = !BettingRound(isPlayerSmallBlind);

            if (handEnded)
            {
                EndHand();
                continue;
            }
                    Console.WriteLine($"Card 1: {computerHand[0]}");
                    Console.WriteLine($"Card 2: {computerHand[1]}");
            DetermineWinner();
            EndHand();
        }

        Console.WriteLine("Game Over");
        Console.WriteLine($"Final Player Stack: {playerStack}");
        Console.WriteLine($"Final Computer Stack: {computerStack}");
        WaitForUserInput();
    }

    static bool BettingRound(bool playerStarts)
    {
        bool actionTaken = false;
        while (true)
        {
            if (playerStarts)
            {
                actionTaken = PlayerAction();
                if (!actionTaken || playerBet == computerBet)
                {
                    break;
                }
                actionTaken = ComputerAction();
                if (!actionTaken || playerBet == computerBet)
                {
                    break;
                }
            }
            else
            {
                actionTaken = ComputerAction();
                if (!actionTaken || playerBet == computerBet)
                {
                    break;
                }
                actionTaken = PlayerAction();
                if (!actionTaken || playerBet == computerBet)
                {
                    break;
                }
            }
        }
        return actionTaken;
    }

    static void EndHand()
    {
        Console.WriteLine("Hand Over");
        Console.WriteLine($"Pot: {pot}");
        Console.WriteLine($"Player Stack: {playerStack}");
        Console.WriteLine($"Computer Stack: {computerStack}");

        // Clear the community cards for the next hand
        Array.Clear(communityCards, 0, communityCards.Length);
        // Reset bets for the next hand
        playerBet = 0;
        computerBet = 0;
        // Switch blinds for the next hand
        isPlayerSmallBlind = !isPlayerSmallBlind;
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
            case 1: // Flop: 3 cards
                communityCards[0] = DealCard();
                communityCards[1] = DealCard();
                communityCards[2] = DealCard();
                break;
            case 2: // Turn: 4 cards (include one more card)
                communityCards[3] = DealCard();
                break;
            case 3: // River: 5 cards (include one more card)
                communityCards[4] = DealCard();
                break;
        }
    }

    static void HandleBlinds()
    {
        if (isPlayerSmallBlind)
        {
            playerStack -= smallBlind;
            computerStack -= bigBlind;
            pot += smallBlind + bigBlind;
            Console.WriteLine($"Player posts small blind of {smallBlind}");
            Console.WriteLine($"Computer posts big blind of {bigBlind}");
        }
        else
        {
            playerStack -= bigBlind;
            computerStack -= smallBlind;
            pot += bigBlind + smallBlind;
            Console.WriteLine($"Computer posts small blind of {smallBlind}");
            Console.WriteLine($"Player posts big blind of {bigBlind}");
        }
    }

    static bool PlayerAction()
    {
        Console.WriteLine("Enter your action: (1) Bet (2) Raise (3) Call (4) Check (5) Fold");

        int action = Convert.ToInt32(Console.ReadLine());
        int callAmount = computerBet - playerBet;

        switch (action)
        {
            case 1: // Bet
            case 2: // Raise
                Console.WriteLine("Enter your bet/raise amount:");
                int amount = Convert.ToInt32(Console.ReadLine());
                if (amount > playerStack)
                {
                    Console.WriteLine("Insufficient funds, betting all-in.");
                    amount = playerStack;
                }
                playerStack -= amount;
                pot += amount;
                playerBet += amount;
                break;
            case 3: // Call
                if (callAmount > playerStack)
                {
                    Console.WriteLine("Insufficient funds, calling all-in.");
                    callAmount = playerStack;
                }
                playerStack -= callAmount;
                pot += callAmount;
                playerBet = computerBet;
                break;
            case 4: // Check
                if (playerBet == computerBet)
                {
                    Console.WriteLine("Checked.");
                }
                else
                {
                    Console.WriteLine("Cannot check; a bet is required.");
                    return PlayerAction(); // Re-prompt action
                }
                break;
            case 5: // Fold
                Console.WriteLine("Player folds.");
                computerStack += pot; // Computer wins the pot
                pot = 0;
                return false; // Indicates hand is over
            default:
                Console.WriteLine("Invalid action.");
                return PlayerAction(); // Re-prompt action
        }
        return true; // Continue if action was valid
    }

    static bool ComputerAction()
    {
        Random rand = new Random();
        int action = rand.Next(1, 6);
        int callAmount = playerBet - computerBet;

        switch (action)
        {
            case 1: // Bet
            case 2: // Raise
                int amount = rand.Next(1, computerStack + 1);
                computerStack -= amount;
                pot += amount;
                computerBet += amount;
                Console.WriteLine($"Computer bets/raises {amount}");
                break;
            case 3: // Call
                if (callAmount > computerStack)
                {
                    callAmount = computerStack;
                }
                computerStack -= callAmount;
                pot += callAmount;
                computerBet = playerBet;
                Console.WriteLine("Computer calls");
                break;
            case 4: // Check
                if (computerBet == playerBet)
                {
                    Console.WriteLine("Computer checks.");
                }
                else
                {
                    Console.WriteLine("Computer cannot check; bet required.");
                    return ComputerAction(); // Re-prompt action
                }
                break;
            case 5: // Fold
                Console.WriteLine("Computer folds.");
                playerStack += pot; // Player wins the pot
                pot = 0;
                return false; // Indicates hand is over
            default:
                Console.WriteLine("Invalid action.");
                return ComputerAction(); // Re-prompt action
        }
        return true; // Continue if action was valid
    }

    static void DisplayPlayerHand()
    {
        Console.WriteLine("Your Hand:");
        Console.WriteLine($"Card 1: {playerHand[0]}");
        Console.WriteLine($"Card 2: {playerHand[1]}");
    }

    static void DisplayCommunityCards()
    {
        Console.WriteLine("Community Cards:");
        for (int i = 0; i < communityCards.Length; i++)
        {
            if (!string.IsNullOrEmpty(communityCards[i]))
            {
                Console.WriteLine($"Card {i + 1}: {communityCards[i]}");
            }
        }
    }

    static void DisplayPot()
    {
        Console.WriteLine($"Pot: {pot}");
    }

    static void DetermineWinner()
    {
        var playerBestHand = GetBestHand(playerHand.Concat(communityCards).ToArray());
        var computerBestHand = GetBestHand(computerHand.Concat(communityCards).ToArray());

        int comparison = CompareHands(playerBestHand, computerBestHand);

        if (comparison > 0)
        {
            Console.WriteLine("Player wins!");
            playerStack += pot;
        }
        else if (comparison < 0)
        {
            Console.WriteLine("Computer wins!");
            computerStack += pot;
        }
        else
        {
            Console.WriteLine("It's a tie!");
            playerStack += pot / 2;
            computerStack += pot / 2;
        }
    }

    static (string Rank, List<int> Values) GetBestHand(string[] cards)
    {
        // Parse the cards into values and suits
        var parsedCards = cards.Select(card => (
            Value: "23456789TJQKA".IndexOf(card[0]) + 2,
            Suit: card[1]
        )).ToList();

        parsedCards.Sort((a, b) => b.Value.CompareTo(a.Value));

        // Group by value and by suit
        var groupsByValue = parsedCards.GroupBy(card => card.Value).OrderByDescending(group => group.Count()).ThenByDescending(group => group.Key).ToList();
        var groupsBySuit = parsedCards.GroupBy(card => card.Suit).ToList();

        bool isFlush = groupsBySuit.Any(group => group.Count() >= 5);
        bool isStraight = false;
        List<int> straightHighCards = new List<int>();

        for (int i = 0; i <= parsedCards.Count - 5; i++)
        {
            if (parsedCards[i].Value - 4 == parsedCards[i + 4].Value &&
                parsedCards[i].Value - 1 == parsedCards[i + 1].Value &&
                parsedCards[i].Value - 2 == parsedCards[i + 2].Value &&
                parsedCards[i].Value - 3 == parsedCards[i + 3].Value)
            {
                isStraight = true;
                straightHighCards.Add(parsedCards[i].Value);
                break;
            }
        }

        if (!isStraight && parsedCards[0].Value == 14)
        {
            var lowStraight = new List<int> { 5, 4, 3, 2, 14 };
            if (parsedCards.Select(card => card.Value).Intersect(lowStraight).Count() == 5)
            {
                isStraight = true;
                straightHighCards.Add(5);
            }
        }

        if (isStraight && isFlush)
        {
            return ("Straight Flush", straightHighCards);
        }

        if (groupsByValue[0].Count() == 4)
        {
            return ("Four of a Kind", groupsByValue.SelectMany(group => group).Take(5).Select(card => card.Value).ToList());
        }

        if (groupsByValue[0].Count() == 3 && groupsByValue[1].Count() >= 2)
        {
            return ("Full House", groupsByValue.SelectMany(group => group).Take(5).Select(card => card.Value).ToList());
        }

        if (isFlush)
        {
            return ("Flush", groupsBySuit.First(group => group.Count() >= 5).Select(card => card.Value).Take(5).ToList());
        }

        if (isStraight)
        {
            return ("Straight", straightHighCards);
        }

        if (groupsByValue[0].Count() == 3)
        {
            return ("Three of a Kind", groupsByValue.SelectMany(group => group).Take(5).Select(card => card.Value).ToList());
        }

        if (groupsByValue[0].Count() == 2 && groupsByValue[1].Count() == 2)
        {
            return ("Two Pair", groupsByValue.SelectMany(group => group).Take(5).Select(card => card.Value).ToList());
        }

        if (groupsByValue[0].Count() == 2)
        {
            return ("One Pair", groupsByValue.SelectMany(group => group).Take(5).Select(card => card.Value).ToList());
        }

        return ("High Card", parsedCards.Take(5).Select(card => card.Value).ToList());
    }

    static int CompareHands((string Rank, List<int> Values) hand1, (string Rank, List<int> Values) hand2)
    {
        string[] handRanks = { "High Card", "One Pair", "Two Pair", "Three of a Kind", "Straight", "Flush", "Full House", "Four of a Kind", "Straight Flush" };
        int rank1 = Array.IndexOf(handRanks, hand1.Rank);
        int rank2 = Array.IndexOf(handRanks, hand2.Rank);

        if (rank1 != rank2)
        {
            return rank1.CompareTo(rank2);
        }

        for (int i = 0; i < hand1.Values.Count; i++)
        {
            if (hand1.Values[i] != hand2.Values[i])
            {
                return hand1.Values[i].CompareTo(hand2.Values[i]);
            }
        }

        return 0;
    }

    static void WaitForUserInput()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    static void LoadGame()
    {
        if (File.Exists(gameFilePath))
        {
            string gameData = File.ReadAllText(gameFilePath);
            Console.WriteLine("Loaded game data:");
            Console.WriteLine(gameData);
        }
        else
        {
            Console.WriteLine("No saved game found.");
        }
        WaitForUserInput();
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
            Console.WriteLine("No saved game found to erase.");
        }
        WaitForUserInput();
    }

    static void ViewGames()
    {
        if (File.Exists(gameFilePath))
        {
            string gameData = File.ReadAllText(gameFilePath);
            Console.WriteLine("Saved games:");
            Console.WriteLine(gameData);
        }
        else
        {
            Console.WriteLine("No saved games found.");
        }
        WaitForUserInput();
    }
}
