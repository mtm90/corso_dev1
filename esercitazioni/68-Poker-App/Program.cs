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
            AnsiConsole.Write(
        new FigletText("Poker App")
        .Centered()
        .Color(Color.Red));
            input = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
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

        RenderGameStatus("Preflop: No community cards yet.");

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
        DealCommunityCards(1); // Correctly deal the three flop cards
        RenderGameStatus("Flop:");
        
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
        DealCommunityCards(2); // Deal the turn card
        RenderGameStatus("Turn:");
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
        DealCommunityCards(3); // Deal the river card
        RenderGameStatus("River:");
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
        RenderGameStatus($"Computer hand: {computerHand[0]} {computerHand[1]}");
        DetermineWinner(currentHand);
        EndHand(currentHand);

        isLoadedGame = false; // Reset after the first loop
    }

    AnsiConsole.Write(new Markup($"[bold]Game Over[/]\nFinal Player Stack: [bold]{playerStack}[/]\nFinal Computer Stack: [bold]{computerStack}[/]"));
    WaitForUserInput();
}


    static void RenderGameStatus(string stage, string actions = "", string potAndStackActions = "")
{
    var layout = new Layout()
        .SplitColumns(
            new Layout("Community Cards").Ratio(3),
            new Layout("RightSide").SplitRows(
            new Layout("Player Hand").Ratio(1),
            new Layout("Pot and Stacks").Ratio(1)
        ));

    layout["Player Hand"].Update(
        new Panel(
            Align.Center(
            new Text(string.Join(" ", playerHand)), VerticalAlignment.Middle))
            .Header("[red]Your Hand[/]", Justify.Center)
            .Border(BoxBorder.Rounded)
            .Expand()
    );

    layout["Community Cards"].Update(
        new Panel(new Markup(" " + string.Join(" ", communityCards.Where(card => card != null)) + "\n" + actions))
            .Header("[yellow]Community Cards and action[/]", Justify.Center)
            .Border(BoxBorder.Rounded)
            .Expand()
    );

    layout["Pot and Stacks"].Update(
        new Panel(
            Align.Center(
                new Markup($"Pot: [bold]{pot}[/]\nYour Stack: [bold]{playerStack}[/]\nComputer Stack: [bold]{computerStack}[/]\n" + potAndStackActions), VerticalAlignment.Middle))
            .Header($"[blue]{stage}[/]", Justify.Center)
            .Border(BoxBorder.Rounded)
            .Expand()
    );

    AnsiConsole.Write(layout);
}


    static void EndHand(HandHistory currentHand)
    {
        currentHand.EndPot = pot;
        currentHand.PlayerStack = playerStack;
        currentHand.ComputerStack = computerStack;
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
    string actions = "";
    while (true)
    {
        actions += "Enter your action: (1) [bold red]Bet/Raise[/] (2) [bold lightgoldenrod1]Call[/] (3) [bold]Check[/] (4) [bold green]Fold[/]\n";
        RenderGameStatus("Player's Turn", actions);
        string input = Console.ReadLine();
        if (int.TryParse(input, out action) && action >= 1 && action <= 4)
        {
            break;
        }
        actions += "[red]Invalid input. Please enter a number between 1 and 4.[/]\n";
        RenderGameStatus("Player's Turn", actions);
    }

    int callAmount = computerBet - playerBet;

    switch (action)
    {
        case 1: // Bet/Raise
            int amount;
            while (true)
            {
                actions += "Enter your bet/raise amount:\n";
                RenderGameStatus("Player's Turn", actions);
                string input = Console.ReadLine();
                if (int.TryParse(input, out amount) && amount >= callAmount && amount <= playerStack)
                {
                    break;
                }
                actions += $"[red]Invalid amount. You must at least call {callAmount} and cannot bet more than your stack.[/]\n";
                RenderGameStatus("Player's Turn", actions);
            }
            playerStack -= amount;
            pot += amount;
            playerBet += amount;
            actions += $"You bet/raised {amount}\n";
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
            actions += $"You call {callAmount}\n";
            currentHand.Actions.Add(new ActionRecord { Actor = "Player", Action = "Call", Amount = callAmount });
            break;

        case 3: // Check
            if (playerBet != computerBet)
            {
                actions += $"[red]You can only check if the current bet is {computerBet}.[/]\n";
                RenderGameStatus("Player's Turn", actions);
                return PlayerAction(isPreflop, currentHand); // Retry if check is not valid
            }
            actions += "You check.\n";
            currentHand.Actions.Add(new ActionRecord { Actor = "Player", Action = "Check" });
            break;

        case 4: // Fold
            actions += "You folded.\n";
            computerStack += pot; // Award the pot to the computer
            currentHand.Actions.Add(new ActionRecord { Actor = "Player", Action = "Fold" });
            return false; // Return false to indicate the player has folded

        default:
            actions += "[red]Invalid action. Try again.[/]\n";
            RenderGameStatus("Player's Turn", actions);
            return PlayerAction(isPreflop, currentHand); // Retry if the action is invalid
    }
    RenderGameStatus("Player's Turn", actions);
    return true; // Return true if the action was valid
}

    static bool ComputerAction(HandHistory currentHand)
{
    Random rand = new Random();
    int action = rand.Next(1, 4); // Randomly choose between Bet/Raise, Call, Check, Fold
    int callAmount = playerBet - computerBet;
    string potAndStackActions = "";

    switch (action)
    {
        case 1: // Bet/Raise
            if (computerStack == 0)
            {
                action = 3; // Change action to Check if the stack is 0
            }
            else
            {
                int amount = rand.Next(playerBet, pot * 2); // Ensure the bet is within the stack limit
                if (amount > computerStack)
                {
                    amount = computerStack; // Bet only the remaining stack if not enough
                }
                computerStack -= amount;
                pot += amount;
                computerBet += amount;
                potAndStackActions += $"Computer bets/raises {amount}\n";
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
            potAndStackActions += $"Computer calls {callAmount}\n";
            currentHand.Actions.Add(new ActionRecord { Actor = "Computer", Action = "Call", Amount = callAmount });
            break;

        case 3: // Check
            if (computerBet != playerBet)
            {
                return ComputerAction(currentHand); // Retry if check is not valid
            }
            potAndStackActions += "Computer checks.\n";
            currentHand.Actions.Add(new ActionRecord { Actor = "Computer", Action = "Check" });
            break;

        case 4: // Fold
            potAndStackActions += "Computer folds.\n";
            playerStack += pot; // Award the pot to the player
            currentHand.Actions.Add(new ActionRecord { Actor = "Computer", Action = "Fold" });
            return false; // Return false to indicate the computer has folded

        default:
            potAndStackActions += "[red]Computer takes an invalid action.[/]\n";
            return ComputerAction(currentHand); // Retry if action is invalid
    }
    RenderGameStatus("Computer's Turn", "", potAndStackActions);
    Thread.Sleep(2500);
    return true; // Return true if the action was valid
}


    static void DetermineWinner(HandHistory currentHand)
    {
        int playerScore = EvaluateHand(playerHand, communityCards, out int playerHighCard, out List<int> playerHighCards);
        int computerScore = EvaluateHand(computerHand, communityCards, out int computerHighCard, out List<int> computerHighCards);

        AnsiConsole.Write(new Markup($"Player Hand Evaluation: Score = {playerScore}, High Cards = {string.Join(", ", playerHighCards)}\n"));
        AnsiConsole.Write(new Markup($"Computer Hand Evaluation: Score = {computerScore}, High Cards = {string.Join(", ", computerHighCards)}\n"));

        if (playerScore > computerScore)
        {
            AnsiConsole.Write(new Markup("[bold]Player wins the hand.[/]\n"));
            playerStack += pot;
            currentHand.Winner = "Player";
        }
        else if (computerScore > playerScore)
        {
            AnsiConsole.Write(new Markup("[bold]Computer wins the hand.[/]\n"));
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
                    AnsiConsole.Write(new Markup("[bold]Player wins the hand[/]\n"));
                    playerStack += pot;
                    currentHand.Winner = "Player";
                    return;
                }
                else if (computerHighCards[i] > playerHighCards[i])
                {
                    AnsiConsole.Write(new Markup("[bold]Computer wins the hand[/]\n"));
                    computerStack += pot;
                    currentHand.Winner = "Computer";
                    return;
                }
            }

            // If all high cards are the same
            AnsiConsole.Write(new Markup("[bold]It's a tie![/]\n"));
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

    static void WaitForUserInput()
    {
        AnsiConsole.Write(new Markup("Press any key to continue...\n"));
        Console.ReadKey();
    }

    static void EraseGame()
    {
        if (File.Exists(gameFilePath))
        {
            File.Delete(gameFilePath);
            AnsiConsole.Write(new Markup("Game data erased.\n"));
        }
        else
        {
            AnsiConsole.Write(new Markup("No game data found to erase.\n"));
        }
        WaitForUserInput();
    }

    static void ViewGameHistory()
    {
        if (File.Exists(gameFilePath))
        {
            string json = File.ReadAllText(gameFilePath);
            GameState gameState = JsonConvert.DeserializeObject<GameState>(json);

            AnsiConsole.Write(new Markup("[bold]Game History:[/]\n"));
            AnsiConsole.Write(new Markup($"Player Stack: [bold]{gameState.PlayerStack}[/]\n"));
            AnsiConsole.Write(new Markup($"Computer Stack: [bold]{gameState.ComputerStack}[/]\n"));
            AnsiConsole.Write(new Markup($"Pot: [bold]{gameState.Pot}[/]\n"));
            AnsiConsole.Write(new Markup($"Player Bet: [bold]{gameState.PlayerBet}[/]\n"));
            AnsiConsole.Write(new Markup($"Computer Bet: [bold]{gameState.ComputerBet}[/]\n"));
            AnsiConsole.Write(new Markup($"Player Hand: [bold]{string.Join(" ", gameState.PlayerHand)}[/]\n"));
            AnsiConsole.Write(new Markup($"Computer Hand: [bold]{string.Join(" ", gameState.ComputerHand)}[/]\n"));
            AnsiConsole.Write(new Markup($"Community Cards: [bold]{string.Join(" ", gameState.CommunityCards)}[/]\n"));
            AnsiConsole.Write(new Markup($"Current Card Index: [bold]{gameState.CurrentCardIndex}[/]\n"));
            AnsiConsole.Write(new Markup($"Is Player Small Blind: [bold]{gameState.IsPlayerSmallBlind}[/]\n"));

            AnsiConsole.Write(new Markup("[bold]Hand Histories:[/]\n"));
            foreach (var hand in gameState.HandHistories)
            {
                AnsiConsole.Write(new Markup("[bold]Hand:[/]\n"));
                AnsiConsole.Write(new Markup($"  Preflop - Player: [bold]{string.Join(" ", hand.Preflop.PlayerHand)}[/], Computer: [bold]{string.Join(" ", hand.Preflop.ComputerHand)}[/], Pot: [bold]{hand.Preflop.Pot}[/]\n"));
                if (hand.Flop != null)
                    AnsiConsole.Write(new Markup($"  Flop - Community: [bold]{string.Join(" ", hand.Flop.CommunityCards)}[/], Pot: [bold]{hand.Flop.Pot}[/]\n"));
                if (hand.Turn != null)
                    AnsiConsole.Write(new Markup($"  Turn - Community: [bold]{string.Join(" ", hand.Turn.CommunityCards)}[/], Pot: [bold]{hand.Turn.Pot}[/]\n"));
                if (hand.River != null)
                    AnsiConsole.Write(new Markup($"  River - Community: [bold]{string.Join(" ", hand.River.CommunityCards)}[/], Pot: [bold]{hand.River.Pot}[/]\n"));
                AnsiConsole.Write(new Markup("[bold]Actions:[/]\n"));
                foreach (var action in hand.Actions)
                {
                    AnsiConsole.Write(new Markup($"    {action.Actor}: {action.Action} {action.Amount}\n"));
                }
                AnsiConsole.Write(new Markup($"  Winner: [bold]{hand.Winner}[/], End Pot: [bold]{hand.EndPot}[/]\n"));
            }
        }
        else
        {
            AnsiConsole.Write(new Markup("No saved game found.\n"));
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

            AnsiConsole.Write(new Markup("Game loaded successfully.\n"));
            WaitForUserInput();
            StartNewGame(true);
        }
        else
        {
            AnsiConsole.Write(new Markup("No saved game found.\n"));
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
