﻿using Newtonsoft.Json;
using Spectre.Console;
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

    static List<HandHistory> handHistories = new List<HandHistory>();

    static void Main()
    {
        string input;
        do
        {
            Console.Clear();
            input = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Welcome to The Poker App! Please choose one of the following options:")
                    .PageSize(5)
                    .AddChoices(new[] { "Start new game", "Load game", "Erase game", "View game history", "Quit" }));

            switch (input)
            {
                case "Start new game":
                    StartNewGame(false);
                    break;
                case "Load game":
                    LoadGame();
                    break;
                case "Erase game":
                    EraseGame();
                    break;
                case "View game history":
                    ViewGameHistory();
                    break;
                case "Quit":
                    Console.WriteLine("Quitting...");
                    break;
                default:
                    Console.WriteLine("Invalid option, please try again");
                    WaitForUserInput();
                    break;
            }
        } while (input != "Quit");
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

            HandHistory currentHand = new HandHistory();
            handHistories.Add(currentHand);

            DealInitialCards();
            HandleBlinds();
            Console.Clear();
            Console.WriteLine("Preflop: No community cards yet.");
            DisplayPlayerHand();
            DisplayPot();
            DisplayStacks();

            currentHand.Preflop = new HandStage
            {
                PlayerHand = (string[])playerHand.Clone(),
                ComputerHand = (string[])computerHand.Clone(),
                PlayerBet = playerBet,
                ComputerBet = computerBet,
                Pot = pot
            };

            bool handEnded = false;
            if (isPlayerSmallBlind)
            {
                handEnded = !BettingRound(true, true, currentHand);
                Thread.Sleep(1500);
            }
            else
            {
                handEnded = !BettingRound(false, true, currentHand);
                Thread.Sleep(1500);
            }

            if (handEnded)
            {
                EndHand(currentHand);
                continue;
            }

            Console.Clear();
            DisplayPlayerHand();
            Console.Write("Flop:");
            DealCommunityCards(1);
            DisplayCommunityCards();
            DisplayPot();
            DisplayStacks();
            currentHand.Flop = new HandStage
            {
                CommunityCards = (string[])communityCards.Clone(),
                PlayerBet = playerBet,
                ComputerBet = computerBet,
                Pot = pot
            };

            if (!isPlayerSmallBlind)
            {
                handEnded = !BettingRound(true, false, currentHand);
                Thread.Sleep(1500);
            }
            else
            {
                handEnded = !BettingRound(false, false, currentHand);
                Thread.Sleep(1500);
            }

            if (handEnded)
            {
                EndHand(currentHand);
                continue;
            }

            Console.Clear();
            DisplayPlayerHand();
            Console.Write("Turn:");
            DealCommunityCards(2);
            DisplayCommunityCards();
            DisplayPot();
            DisplayStacks();
            currentHand.Turn = new HandStage
            {
                CommunityCards = (string[])communityCards.Clone(),
                PlayerBet = playerBet,
                ComputerBet = computerBet,
                Pot = pot
            };

            if (!isPlayerSmallBlind)
            {
                handEnded = !BettingRound(true, false, currentHand);
                Thread.Sleep(1500);
            }
            else
            {
                handEnded = !BettingRound(false, false, currentHand);
                Thread.Sleep(1500);
            }

            if (handEnded)
            {
                EndHand(currentHand);
                continue;
            }
            Console.Clear();
            DisplayPlayerHand();
            Console.Write("River:");
            DealCommunityCards(3);
            DisplayCommunityCards();
            DisplayPot();
            DisplayStacks();
            currentHand.River = new HandStage
            {
                CommunityCards = (string[])communityCards.Clone(),
                PlayerBet = playerBet,
                ComputerBet = computerBet,
                Pot = pot
            };

            if (!isPlayerSmallBlind)
            {
                handEnded = !BettingRound(true, false, currentHand);
                Thread.Sleep(1500);
            }
            else
            {
                handEnded = !BettingRound(false, false, currentHand);
                Thread.Sleep(1500);
            }

            if (handEnded)
            {
                EndHand(currentHand);
                continue;
            }
            Console.Clear();
            Console.WriteLine($"Computer hand: {computerHand[0]} {computerHand[1]}");
            DisplayPlayerHand();
            DisplayCommunityCards();
            DetermineWinner(currentHand);
            EndHand(currentHand);

            isLoadedGame = false; // Reset after the first loop
        }

        Console.WriteLine("Game Over");
        Console.WriteLine($"Final Player Stack: {playerStack}");
        Console.WriteLine($"Final Computer Stack: {computerStack}");
        WaitForUserInput();
    }

    static void EndHand(HandHistory currentHand)
    {
        currentHand.EndPot = pot;
        currentHand.PlayerStack = playerStack;
        currentHand.ComputerStack = computerStack;

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

        }
        else
        {
            playerStack -= bigBlind;
            playerBet = bigBlind;
            computerStack -= smallBlind;
            computerBet = smallBlind;
            pot += bigBlind + smallBlind;

        }
    }

    static bool BettingRound(bool playerStarts, bool isPreflop, HandHistory currentHand)
{
    // Indicates if the betting round is over
    bool roundOver = false;
    
    // Determine whose turn it is initially based on the playerStarts parameter
    bool playerTurn = playerStarts;

    // Continue the loop until the betting round is concluded
    while (!roundOver)
    {
        if (playerTurn)
        {
            // If it's the player's turn, execute the player's action
            // If the player folds (returns false), the round ends immediately
            if (!PlayerAction(isPreflop, currentHand)) return false;
        }
        else
        {
            // If it's the computer's turn, execute the computer's action
            // If the computer folds (returns false), the round ends immediately
            if (!ComputerAction(currentHand)) return false;
        }

        // Switch turns: if it was the player's turn, it becomes the computer's turn and vice versa
        playerTurn = !playerTurn;

        // Check if the round is over: this happens when both bets are equal and both players have acted
        // We check if it's not the player's turn to ensure both have acted at least once after equalizing the bets
        if (playerBet == computerBet && !playerTurn)
        {
            roundOver = true;
        }
    }

    // Return true indicating the round completed without any player folding
    return true;
}



    static bool PlayerAction(bool isPreflop, HandHistory currentHand)
{
    int action;
    while (true)
    {
        Console.WriteLine("Enter your action: (1) Bet/Raise (2) Call (3) Check (4) Fold");
        string input = Console.ReadLine();
        if (int.TryParse(input, out action) && action >= 1 && action <= 4)
        {
            break;
        }
        Console.WriteLine("Invalid input. Please enter a number between 1 and 4.");
    }

    int callAmount = computerBet - playerBet;

    switch (action)
    {
        case 1: // Bet/Raise
            int amount;
            while (true)
            {
                Console.WriteLine("Enter your bet/raise amount:");
                string input = Console.ReadLine();
                if (int.TryParse(input, out amount) && amount >= callAmount && amount <= playerStack)
                {
                    break;
                }
                Console.WriteLine($"Invalid amount. You must at least call {callAmount} and cannot bet more than your stack.");
            }
            playerStack -= amount;
            pot += amount;
            playerBet += amount;
            Console.WriteLine($"You bet/raised {amount}");
            currentHand.Actions.Add(new ActionRecord { Actor = "Player", Action = "Bet/Raise", Amount = amount });
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
            currentHand.Actions.Add(new ActionRecord { Actor = "Player", Action = "Call", Amount = callAmount });
            break;

        case 3: // Check
            if (playerBet != computerBet)
            {
                Console.WriteLine($"You can only check if the current bet is {computerBet}.");
                return PlayerAction(isPreflop, currentHand); // Retry if check is not valid
            }
            Console.WriteLine("You check.");
            currentHand.Actions.Add(new ActionRecord { Actor = "Player", Action = "Check" });
            break;

        case 4: // Fold
            Console.WriteLine("You folded.");
            computerStack += pot; // Award the pot to the computer
            currentHand.Actions.Add(new ActionRecord { Actor = "Player", Action = "Fold" });
            return false; // Return false to indicate the player has folded

        default:
            Console.WriteLine("Invalid action. Try again.");
            return PlayerAction(isPreflop, currentHand); // Retry if the action is invalid
    }
    return true; // Return true if the action was valid
}


    static bool ComputerAction(HandHistory currentHand)
{
    Random rand = new Random();
    int action = rand.Next(1, 4); // Randomly choose between Bet/Raise, Call, Check, Fold
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
                int amount = rand.Next(bigBlind, pot * 2); // Ensure the bet is within the stack limit
                if (amount > computerStack)
                {
                    amount = computerStack; // Bet only the remaining stack if not enough
                }
                computerStack -= amount;
                pot += amount;
                computerBet += amount;
                Console.WriteLine($"Computer bets/raises {amount}");
                currentHand.Actions.Add(new ActionRecord { Actor = "Computer", Action = "Bet/Raise", Amount = amount });
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
            currentHand.Actions.Add(new ActionRecord { Actor = "Computer", Action = "Call", Amount = callAmount });
            break;

        case 3: // Check
            if (computerBet != playerBet)
            {
                return ComputerAction(currentHand); // Retry if check is not valid
            }
            Console.WriteLine("Computer checks.");
            currentHand.Actions.Add(new ActionRecord { Actor = "Computer", Action = "Check" });
            break;

        case 4: // Fold
            Console.WriteLine("Computer folds.");
            playerStack += pot; // Award the pot to the player
            currentHand.Actions.Add(new ActionRecord { Actor = "Computer", Action = "Fold" });
            return false; // Return false to indicate the computer has folded

        default:
            Console.WriteLine("Computer takes an invalid action.");
            return ComputerAction(currentHand); // Retry if action is invalid
    }
    return true; // Return true if the action was valid
}


    static void DetermineWinner(HandHistory currentHand)
{
    int playerScore = EvaluateHand(playerHand, communityCards, out int playerHighCard, out List<int> playerHighCards);
    int computerScore = EvaluateHand(computerHand, communityCards, out int computerHighCard, out List<int> computerHighCards);

    Console.WriteLine($"Player Hand Evaluation: Score = {playerScore}, High Cards = {string.Join(", ", playerHighCards)}");
    Console.WriteLine($"Computer Hand Evaluation: Score = {computerScore}, High Cards = {string.Join(", ", computerHighCards)}");

    if (playerScore > computerScore)
    {
        Console.WriteLine("Player wins the hand.");
        playerStack += pot;
        currentHand.Winner = "Player";
    }
    else if (computerScore > playerScore)
    {
        Console.WriteLine("Computer wins the hand.");
        computerStack += pot;
        currentHand.Winner = "Computer";
    }
    else
    {
        // When scores are equal, compare the high cards
        for (int i = 0; i < playerHighCards.Count; i++)
        {
            if (playerHighCards[i] > computerHighCards[i])
            {
                Console.WriteLine("Player wins the hand");
                playerStack += pot;
                currentHand.Winner = "Player";
                return;
            }
            else if (computerHighCards[i] > playerHighCards[i])
            {
                Console.WriteLine("Computer wins the hand");
                computerStack += pot;
                currentHand.Winner = "Computer";
                return;
            }
        }

        // If all high cards are the same
        Console.WriteLine("It's a tie!");
        playerStack += pot / 2;
        computerStack += pot / 2;
        currentHand.Winner = "Tie";
    }
}

static int EvaluateHand(string[] hand, string[] communityCards, out int highCard, out List<int> highCards)
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

    // Create a list of cards sorted by their values
    var sortedCards = allCards.Select(card => new { Value = GetCardValue(card.Length == 3 ? card.Substring(0, 2) : card[0].ToString()), Suit = card.Last() })
                              .OrderByDescending(card => card.Value)
                              .ToList();

    // Evaluate the best 5-card hand
    highCards = sortedCards.Take(5).Select(card => card.Value).ToList();
    highCard = highCards.First();

    if (IsStraightFlush(cardValues, cardSuits)) { highCard = highCards[0]; return 9; }
    if (IsFourOfAKind(cardValues)) { highCard = highCards[0]; return 8; }
    if (IsFullHouse(cardValues)) { highCard = highCards[0]; return 7; }
    if (IsFlush(cardSuits)) { highCard = highCards[0]; return 6; }
    if (IsStraight(cardValues)) { highCard = highCards[0]; return 5; }
    if (IsThreeOfAKind(cardValues)) { highCard = highCards[0]; return 4; }
    if (IsTwoPair(cardValues, out highCard)) { return 3; }
    if (IsOnePair(cardValues, out highCard)) { return 2; }

    highCard = highCards[0];
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
        Console.WriteLine(" " + string.Join(" ", communityCards.Where(card => card != null)));
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

            Console.WriteLine("Hand Histories:");
            foreach (var hand in gameState.HandHistories)
            {
                Console.WriteLine($"Hand:");
                Console.WriteLine($"  Preflop - Player: {string.Join(" ", hand.Preflop.PlayerHand)}, Computer: {string.Join(" ", hand.Preflop.ComputerHand)}, Pot: {hand.Preflop.Pot}");
                if (hand.Flop != null)
                    Console.WriteLine($"  Flop - Community: {string.Join(" ", hand.Flop.CommunityCards)}, Pot: {hand.Flop.Pot}");
                if (hand.Turn != null)
                    Console.WriteLine($"  Turn - Community: {string.Join(" ", hand.Turn.CommunityCards)}, Pot: {hand.Turn.Pot}");
                if (hand.River != null)
                    Console.WriteLine($"  River - Community: {string.Join(" ", hand.River.CommunityCards)}, Pot: {hand.River.Pot}");
                Console.WriteLine($"  Actions:");
                foreach (var action in hand.Actions)
                {
                    Console.WriteLine($"    {action.Actor}: {action.Action} {action.Amount}");
                }
                Console.WriteLine($"  Winner: {hand.Winner}, End Pot: {hand.EndPot}");
            }
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
            IsPlayerSmallBlind = isPlayerSmallBlind,
            HandHistories = handHistories
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
            handHistories = gameState.HandHistories;

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
        public List<HandHistory> HandHistories { get; set; } = new List<HandHistory>();
    }

    class HandHistory
    {
        public HandStage Preflop { get; set; }
        public HandStage Flop { get; set; }
        public HandStage Turn { get; set; }
        public HandStage River { get; set; }
        public List<ActionRecord> Actions { get; set; } = new List<ActionRecord>();
        public string Winner { get; set; }
        public int EndPot { get; set; }
        public int PlayerStack { get; set; }
        public int ComputerStack { get; set; }
    }

    class HandStage
    {
        public string[] PlayerHand { get; set; }
        public string[] ComputerHand { get; set; }
        public string[] CommunityCards { get; set; }
        public int PlayerBet { get; set; }
        public int ComputerBet { get; set; }
        public int Pot { get; set; }
    }

    class ActionRecord
    {
        public string Actor { get; set; }
        public string Action { get; set; }
        public int Amount { get; set; } = 0;
    }
}