using Newtonsoft.Json;
using Spectre.Console;
using System.Data;
using System.Data.SQLite;

class Program
{
    static string[] deck = new string[52];
    static string[] playerHand = new string[2];
    static string[] computerHand = new string[2];
    static string[] communityCards = new string[5];
    static int currentCardIndex = 0;
    static int playerStack = 500;
    static int computerStack = 500;
    static int smallBlind = 1;
    static int bigBlind = 2;
    static int pot = 0;
    static int playerBet = 0;
    static int computerBet = 0;
    static bool isPlayerSmallBlind = true;
    static string gameFilePath = "game_data.json";

    static string playerName; // Declare playerName globally

    static string databasePath = "gameDatabase.db"; // Path to your SQLite database


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
        Console.Write("Please enter your name: ");
        playerName = Console.ReadLine()!;
        if (string.IsNullOrWhiteSpace(playerName))
        {
            Console.WriteLine("Name cannot be empty. Please enter a valid name.");
        }
        CreateDatabaseAndTable();
        InsertPlayerName(playerName);
        string connectionString = $"Data Source={databasePath};Version=3;";
        using (var connection = new SQLiteConnection(connectionString))
        {

         connection.Open();

// Modify hands table creation to add won_hand column (BOOL)
        string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS hands (
                hand_id INTEGER PRIMARY KEY AUTOINCREMENT,
                player_id INTEGER NOT NULL,
                player_hand TEXT NOT NULL,
                date TEXT NOT NULL,
                won_hand BOOL NOT NULL,
                FOREIGN KEY (player_id) REFERENCES players(player_id)
            );
        ";


            
            using (var command = new SQLiteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        }


        if (!isLoadedGame)
        {
            playerStack = 500;
            computerStack = 500;
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
                PlayerBet = playerBet,
                ComputerBet = computerBet,
                Pot = pot
            };

            bool handEnded = false;
            if (isPlayerSmallBlind)
            {
                handEnded = !BettingRound(true, true, currentHand);
                Thread.Sleep(1000);
            }
            else
            {
                handEnded = !BettingRound(false, true, currentHand);
                Thread.Sleep(1000);
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
                Thread.Sleep(1000);
            }
            else
            {
                handEnded = !BettingRound(false, false, currentHand);
                Thread.Sleep(1000);
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
                Thread.Sleep(1000);
            }
            else
            {
                handEnded = !BettingRound(false, false, currentHand);
                Thread.Sleep(1000);
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
                Thread.Sleep(1000);
            }
            else
            {
                handEnded = !BettingRound(false, false, currentHand);
                Thread.Sleep(1000);
            }

            if (handEnded)
            {
                EndHand(currentHand);
                continue;
            }
            Console.Clear();
            DetermineWinner(currentHand);
            EndHand(currentHand);

            isLoadedGame = false; // Reset after the first loop
        }
        if (playerStack == 0)
        {
            RenderGameStatus("Game over: Computer Wins!");
            Thread.Sleep(5000);
        }
        else if (computerStack == 0)
        {
            RenderGameStatus("Game over: Player Wins!");
            Thread.Sleep(5000);
        }

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
            new Panel(
                Align.Center(
                    new Markup(" " + string.Join(" ", communityCards.Where(card => card != null)) + "\n" + actions), VerticalAlignment.Middle))
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

    // Determine if the player won
    bool playerWon = currentHand.Winner == "Player";

    // Save the game and pass the result of whether the player won
    SaveGame(playerName, playerWon);
}



    static void InitializeDeck()
    {
        string[] suits = { "♠", "♥", "♦", "♣" };
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
        bool roundOver = false; // Indicates if the betting round is over
        bool playerTurn = playerStarts; // Determines whose turn it is to act
        bool firstAction = true; // Flag to track the first action in the round

        // Loop until the round is over
        while (!roundOver)
        {
            // Player's turn
            if (playerTurn)
            {
                // Player action method returns false if the player folds
                if (!PlayerAction(isPreflop, currentHand))
                {
                    return false; // End the round immediately
                }
            }
            // Computer's turn
            else
            {
                // Computer action method returns false if the computer folds
                if (!ComputerAction(currentHand))
                {
                    return false; // End the round immediately
                }
            }

            // Switch turns: Player turn becomes Computer's and vice versa
            playerTurn = !playerTurn;

            // Check if the round is over
            if ((playerBet == computerBet && !playerTurn) || (computerBet == playerBet && playerTurn))
            {
                // For preflop, check if it's the first action
                if (firstAction)
                {
                    // For preflop, ensure that both players have acted
                    // After both players have acted once, end the round
                    firstAction = false; // Mark that the first action is done
                }
                else
                {
                    // In postflop, just check if bets are matched
                    roundOver = true;
                }
            }
            else
            {
                // After the first action, no need to ensure both have acted
                firstAction = false;
            }
        }

        return true; // Round completed without folding, proceed to the next stage
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
                    actions += $"[red]You can't check and must at least call {computerBet}.[/]\n";
                    Thread.Sleep(1000);
                    RenderGameStatus("Player's Turn", actions);
                    return PlayerAction(isPreflop, currentHand); // Retry if check is not valid
                }
                actions += "You check.\n";
                currentHand.Actions.Add(new ActionRecord { Actor = "Player", Action = "Check" });
                break;

            case 4: // Fold
                actions += "You folded. Computer Wins\n";
                RenderGameStatus("Player's Turn", actions);
                Thread.Sleep(1500);
                computerStack += pot; // Award the pot to the computer
                currentHand.Actions.Add(new ActionRecord { Actor = "Player", Action = "Fold" });
                return false;

            default:
                actions += "[red]Invalid action. Try again.[/]\n";
                RenderGameStatus("Player's Turn", actions);
                return PlayerAction(isPreflop, currentHand); // Retry if the action is invalid
        }
        RenderGameStatus("Player's Turn", actions);
        Thread.Sleep(2000);
        return true; // Return true if the action was valid
    }

    static bool ComputerAction(HandHistory currentHand)
    {
        Random rand = new Random();
        int action = rand.Next(1, 4); // Randomly choose between Bet/Raise, Call, Check
        int callAmount = playerBet - computerBet;
        string actions = "";

        switch (action)
        {
            case 1: // Bet/Raise
                if (computerStack == 0)
                {
                    action = 3; // Change action to Check if the stack is 0
                }
                else
                {
                    int amount = rand.Next(playerBet * 2, pot * 2); // Ensure the bet is within the stack limit
                    if (amount > computerStack)
                    {
                        amount = computerStack; // Bet only the remaining stack if not enough
                    }
                    computerStack -= amount;
                    pot += amount;
                    computerBet += amount;
                    actions += $"Computer bets/raises {amount}\n";
                    RenderGameStatus("Computer's Turn", actions);
                    currentHand.Actions.Add(new ActionRecord { Actor = "Computer", Action = "Bet/Raise", Amount = amount });
                }
                break;

            case 2: // Call
                if (callAmount > computerStack)
                {
                    callAmount = computerStack; // Call only the remaining stack if not enough
                }
                if (callAmount == 0)
                {
                    return ComputerAction(currentHand); // Retry if check is not valid

                }
                computerStack -= callAmount;
                pot += callAmount;
                computerBet = playerBet;
                actions += $"Computer calls {callAmount}\n";
                RenderGameStatus("Computer's Turn", actions);
                currentHand.Actions.Add(new ActionRecord { Actor = "Computer", Action = "Call", Amount = callAmount });
                break;

            case 3: // Check
                if (computerBet != playerBet)
                {
                    return ComputerAction(currentHand); // Retry if check is not valid
                }
                actions += "Computer checks.\n";
                RenderGameStatus("Computer's Turn", actions);
                currentHand.Actions.Add(new ActionRecord { Actor = "Computer", Action = "Check" });
                break;

            case 4: // Fold
                actions += "Computer folds. Player Wins\n";
                RenderGameStatus("Computer's Turn", actions);
                playerStack += pot; // Award the pot to the player
                currentHand.Actions.Add(new ActionRecord { Actor = "Computer", Action = "Fold" });
                return false; // Return false to indicate the computer has folded

            default:
                actions += "[red]Computer takes an invalid action.[/]\n";
                RenderGameStatus("Computer's Turn", actions);
                return ComputerAction(currentHand); // Retry if action is invalid
        }
        RenderGameStatus("Computer's Turn", actions);
        Thread.Sleep(2000);
        return true; // Return true if the action was valid
    }



    static void DetermineWinner(HandHistory currentHand)
    {
        // Arrays to store high cards
        List<int> playerHighCards;
        List<int> computerHighCards;

        // Evaluate hands
        int playerScore = EvaluateHand(playerHand, communityCards, out int playerHighCard, out playerHighCards);
        int computerScore = EvaluateHand(computerHand, communityCards, out int computerHighCard, out computerHighCards);

        // Array of hand descriptions
        string[] stringScores = {
        "High card", "One pair", "Two Pair", "Three of a kind",
        "Straight", "Flush", "Full House", "Four of a kind",
        "Straight Flush"
    };

        // Determine hand evaluations
        string playerEvaluation = $"Player has {stringScores[playerScore - 1]}";
        string computerEvaluation = $"Computer has {stringScores[computerScore - 1]}";

        // Construct messages
        string actions = $"{playerEvaluation}\n{computerEvaluation}\n";

        // Determine the winner
        if (playerScore > computerScore)
        {
            actions += "[red bold]Player wins the hand.[/]\n";
            playerStack += pot;
            currentHand.Winner = "Player";
        }
        else if (computerScore > playerScore)
        {
            actions += "[blue bold]Computer wins the hand.[/]\n";
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
                    actions += "[red bold]Player wins the hand[/]\n";
                    playerStack += pot;
                    currentHand.Winner = "Player";
                    RenderGameStatus($"Computer hand: {computerHand[0]} {computerHand[1]}", actions);
                    Thread.Sleep(5000);
                    WaitForUserInput();
                    return;
                }
                else if (computerHighCards[i] > playerHighCards[i])
                {
                    actions += "[blue bold]Computer wins the hand[/]\n";
                    computerStack += pot;
                    currentHand.Winner = "Computer";
                    RenderGameStatus($"Computer hand: {computerHand[0]} {computerHand[1]}", actions);
                    Thread.Sleep(5000);
                    WaitForUserInput();
                    return;
                }
            }

            // If all high cards are the same
            actions += "[bold]It's a tie![/]\n";
            playerStack += pot / 2;
            computerStack += pot / 2;
            currentHand.Winner = "Tie";
        }

        RenderGameStatus($"Computer hand: {computerHand[0]} {computerHand[1]}", actions);
        Thread.Sleep(5000);
        WaitForUserInput();
    }



    static int EvaluateHand(string[] hand, string[] communityCards, out int highCard, out List<int> highCards)
    {
        // Combine the player's hand and community cards into a single list of all available cards
        List<string> allCards = new List<string>(hand);
        allCards.AddRange(communityCards);

        // Arrays to hold the values and suits of all cards
        string[] cardValues = new string[allCards.Count];
        char[] cardSuits = new char[allCards.Count];

        // Separate the card values and suits
        for (int i = 0; i < allCards.Count; i++)
        {
            string card = allCards[i];
            // Extract card value (handling the possibility of a two-character value)
            cardValues[i] = card.Length == 3 ? card.Substring(0, 2) : card[0].ToString();
            // Extract the suit of the card (last character)
            cardSuits[i] = card.Last();
        }

        // Sort the card values in descending order for hand evaluation
        Array.Sort(cardValues, (a, b) => GetCardValue(b).CompareTo(GetCardValue(a)));
        // Sort the card suits to evaluate flushes
        Array.Sort(cardSuits);

        // Create a list of cards sorted by their values (for evaluation purposes)
        var sortedCards = allCards.Select(card => new { Value = GetCardValue(card.Length == 3 ? card.Substring(0, 2) : card[0].ToString()), Suit = card.Last() })
                                  .OrderByDescending(card => card.Value)
                                  .ToList();

        // Extract the top 5 cards based on their values
        highCards = sortedCards.Take(5).Select(card => card.Value).ToList();
        // The highest card value in the best hand
        highCard = highCards.First();

        // Determine the hand ranking based on poker hand rules
        if (IsStraightFlush(cardValues, cardSuits)) { highCard = highCards[0]; return 9; }
        if (IsFourOfAKind(cardValues)) { highCard = highCards[0]; return 8; }
        if (IsFullHouse(cardValues)) { highCard = highCards[0]; return 7; }
        if (IsFlush(cardSuits)) { highCard = highCards[0]; return 6; }
        if (IsStraight(cardValues)) { highCard = highCards[0]; return 5; }
        if (IsThreeOfAKind(cardValues)) { highCard = highCards[0]; return 4; }
        if (IsTwoPair(cardValues, out highCard)) { return 3; }
        if (IsOnePair(cardValues, out highCard)) { return 2; }

        // If none of the above hand rankings are matched, the hand is a high card
        highCard = highCards[0];
        return 1;
    }



    static int GetCardValue(string card)
    {
        // This function maps card face values to their corresponding numeric values.
        // It handles values from "2" to "10" and face cards "J", "Q", "K", "A".
        // The numeric value for each card is returned, where Ace (A) is the highest with a value of 14.
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
            default: throw new Exception("Invalid card value"); // Throws an exception if the card value is not recognized.
        }
    }

    static bool IsStraightFlush(string[] cardValues, char[] cardSuits)
    {
        // This function checks if the hand is a Straight Flush.
        // A Straight Flush is a hand where all cards are in sequence and of the same suit.
        // It returns true if both IsFlush and IsStraight conditions are met, otherwise false.
        return IsFlush(cardSuits) && IsStraight(cardValues);
    }


    static bool IsFourOfAKind(string[] cardValues)
    {
        // This function checks if the hand contains Four of a Kind.
        // Four of a Kind is a hand where four cards have the same value.
        // It iterates through the card values and checks for a sequence of four identical values.
        for (int i = 0; i < cardValues.Length - 3; i++)
        {
            if (cardValues[i] == cardValues[i + 1] && cardValues[i] == cardValues[i + 2] && cardValues[i] == cardValues[i + 3])
            {
                return true; // Four of a Kind found.
            }
        }
        return false; // No Four of a Kind found.
    }


    static bool IsFullHouse(string[] cardValues)
    {
        // This function checks if the hand is a Full House.
        // A Full House is a hand where there is a Three of a Kind and a Pair.
        var valueCounts = new Dictionary<string, int>();

        // Count occurrences of each card value
        foreach (var value in cardValues)
        {
            if (valueCounts.ContainsKey(value))
                valueCounts[value]++;
            else
                valueCounts[value] = 1;
        }

        bool hasThreeOfAKind = false;
        bool hasPair = false;

        // Check the counts for Three of a Kind and Pair
        foreach (var count in valueCounts.Values)
        {
            if (count == 3)
                hasThreeOfAKind = true;
            else if (count == 2)
                hasPair = true;
            else if (count == 4)
            {
                // If we have four of a kind, it counts as both Three of a Kind and a Pair
                hasThreeOfAKind = true;
                hasPair = true;
            }
        }

        return hasThreeOfAKind && hasPair; // Return true if both conditions are met.
    }



    static bool IsFlush(char[] cardSuits)
    {
        // This function checks if the hand is a Flush.
        // A Flush is a hand where all cards are of the same suit.
        // It iterates through the suits to see if there are five consecutive cards of the same suit.
        for (int i = 0; i < cardSuits.Length - 4; i++)
        {
            if (cardSuits[i] == cardSuits[i + 1] && cardSuits[i] == cardSuits[i + 2] && cardSuits[i] == cardSuits[i + 3] && cardSuits[i] == cardSuits[i + 4])
            {
                return true; // Flush found.
            }
        }
        return false; // No Flush found.
    }


    static bool IsStraight(string[] cardValues)
    {
        // This function checks if the hand is a Straight.
        // A Straight is a hand where all cards are in sequence.
        int[] cardRanks = cardValues.Select(GetCardValue).ToArray();
        Array.Sort(cardRanks);
        cardRanks = cardRanks.Distinct().ToArray(); // Remove duplicates to handle cases like 2-2-3-4-5.

        // Check for Ace-low Straight (A-2-3-4-5)
        if (cardRanks.Length >= 5 && cardRanks[0] == 2 && cardRanks[1] == 3 && cardRanks[2] == 4 && cardRanks[3] == 5 && cardRanks.Contains(14))
        {
            return true;
        }

        // Check for general Straight
        for (int i = 0; i < cardRanks.Length - 4; i++)
        {
            if (cardRanks[i + 4] - cardRanks[i] == 4)
            {
                return true; // Straight found.
            }
        }
        return false; // No Straight found.
    }



    static bool IsThreeOfAKind(string[] cardValues)
    {
        // This function checks if the hand contains Three of a Kind.
        // Three of a Kind is a hand where three cards have the same value.
        for (int i = 0; i < cardValues.Length - 2; i++)
        {
            if (cardValues[i] == cardValues[i + 1] && cardValues[i] == cardValues[i + 2])
            {
                return true; // Three of a Kind found.
            }
        }
        return false; // No Three of a Kind found.
    }


    static bool IsTwoPair(string[] cardValues, out int highCard)
    {
        // This function checks if the hand contains Two Pair.
        // Two Pair is a hand with two different pairs.
        List<int> pairs = new List<int>();
        int remainingCard = 0;

        // Iterate through card values to find pairs
        for (int i = 0; i < cardValues.Length - 1; i++)
        {
            if (cardValues[i] == cardValues[i + 1])
            {
                pairs.Add(GetCardValue(cardValues[i]));
                i++; // Skip the next card as it's part of the pair
            }
            else if (pairs.Count < 2) // Track remaining card for kicker
            {
                remainingCard = GetCardValue(cardValues[i]);
            }
        }

        if (pairs.Count >= 2)
        {
            pairs.Sort();
            pairs.Reverse(); // Highest pair first
            highCard = pairs.First(); // The highest pair's card value

            // If the two highest pairs are equal, check the next highest card (kicker)
            if (pairs.Count > 1 && pairs[0] == pairs[1])
            {
                highCard = Math.Max(pairs[0], Math.Max(remainingCard, GetCardValue(cardValues.Last())));
            }
            else
            {
                highCard = pairs[0]; // Highest pair value
            }

            return true; // Two Pair found.
        }

        highCard = 0; // No Two Pair found.
        return false;
    }



    static bool IsOnePair(string[] cardValues, out int highCard)
    {
        // This function checks if the hand contains One Pair.
        // One Pair is a hand where there is exactly one pair of cards with the same value.

        int highestPairValue = 0;
        bool foundPair = false;

        // Iterate through the card values to find a pair
        for (int i = 0; i < cardValues.Length - 1; i++)
        {
            if (cardValues[i] == cardValues[i + 1])
            {
                // Get the card value for the pair
                int pairValue = GetCardValue(cardValues[i]);
                if (pairValue > highestPairValue)
                {
                    // Update highestPairValue if this pair is higher
                    highestPairValue = pairValue;
                    foundPair = true;
                }
                i++; // Skip the next card as it's part of the pair
            }
        }

        // If a pair was found, set highCard to the value of the highest pair
        if (foundPair)
        {
            highCard = highestPairValue;
            return true; // One Pair found.
        }

        highCard = 0; // No One Pair found.
        return false;
    }



    static void WaitForUserInput(string actions = "")
    {
        actions += "Press any key to continue...\n";
        RenderGameStatus(actions);
        Console.ReadKey(true);
    }

    static void EraseGame()
    {
        try
        {
            if (File.Exists(gameFilePath))
            {
                File.Delete(gameFilePath);
                AnsiConsole.Write(new Markup("Game data erased.\n"));
                Thread.Sleep(1500);

            }
            else
            {
                AnsiConsole.Write(new Markup("No game data found to erase.\n"));
                Thread.Sleep(1500);
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.Write(new Markup($"[red]Error erasing game data: {ex.Message}[/]\n"));
            Thread.Sleep(6500);

        }
        WaitForUserInput();
    }


    static void ViewGameHistory()
    {
        try
        {
            if (File.Exists(gameFilePath))
            {
                string json = File.ReadAllText(gameFilePath);
                GameState gameState = JsonConvert.DeserializeObject<GameState>(json)!;

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
        }
        catch (Exception ex)
        {
            AnsiConsole.Write(new Markup($"[red]Error viewing game history: {ex.Message}[/]\n"));
        }
        AnsiConsole.WriteLine("Press any key to continue");
        Console.ReadKey(true);
    }


   static void SaveGame(string playerName, bool playerWon)
{
    try
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

        string connectionString = $"Data Source={databasePath};Version=3;";
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            // Get the player_id of the current player based on the playerName variable
            int playerId = GetPlayerId(playerName, connection);

            // Insert the player hand with the player_id as a foreign key and won_hand status
            string insertQuery = @"
                INSERT INTO hands (player_id, player_hand, date, won_hand)
                VALUES (@playerId, @playerHand, @date, @wonHand);
            ";

            using (var command = new SQLiteCommand(insertQuery, connection))
            {
                // Store playerHand and communityCards as JSON or separated by commas
                string playerHandString = $"{playerHand[0]}{playerHand[1]}";
                string time = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

                command.Parameters.AddWithValue("@playerId", playerId);
                command.Parameters.AddWithValue("@playerHand", playerHandString);
                command.Parameters.AddWithValue("@date", time);
                command.Parameters.AddWithValue("@wonHand", playerWon ? 1 : 0); // Store 1 for win (yes) and 0 for loss (no)
                command.ExecuteNonQuery();
            }
        }

        // Save game state to JSON file
        string json = JsonConvert.SerializeObject(gameState, Formatting.Indented);
        File.WriteAllText(gameFilePath, json);
    }
    catch (Exception ex)
    {
        AnsiConsole.Write(new Markup($"[red]Error saving game: {ex.Message}[/]\n"));
    }
}


    static int GetPlayerId(string playerName, SQLiteConnection connection)
{
    // Query the database to retrieve the player_id for the given player_name
    string query = "SELECT player_id FROM players WHERE player_name = @playerName";
    using (var command = new SQLiteCommand(query, connection))
    {
        command.Parameters.AddWithValue("@playerName", playerName);
        object result = command.ExecuteScalar();

        if (result != null && int.TryParse(result.ToString(), out int playerId))
        {
            return playerId;
        }
        else
        {
            throw new Exception("Player not found");
        }
    }
}



    static void LoadGame()
    {
        try
        {
            if (File.Exists(gameFilePath))
            {
                string json = File.ReadAllText(gameFilePath);
                GameState gameState = JsonConvert.DeserializeObject<GameState>(json)!;

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
                Thread.Sleep(2500);
                WaitForUserInput();
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.Write(new Markup($"[red]Error loading game: {ex.Message}[/]\n"));
            WaitForUserInput();
        }
    }
    static void CreateDatabaseAndTable()
    {
        // Connection string for SQLite
        string connectionString = $"Data Source={databasePath};Version=3;";

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            // Create table query
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS players (
                    player_id INTEGER PRIMARY KEY AUTOINCREMENT,
                    player_name TEXT NOT NULL
                );
            ";

            using (var command = new SQLiteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    static void InsertPlayerName(string playerName)
    {
        // Connection string for SQLite
        string connectionString = $"Data Source={databasePath};Version=3;";

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            // Insert player name query
            string insertQuery = @"
                INSERT INTO players (player_name)
                VALUES (@playerName);
            ";

            using (var command = new SQLiteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@playerName", playerName);
                command.ExecuteNonQuery();
            }
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

