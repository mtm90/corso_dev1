using Newtonsoft.Json;
class Program
{
    // Arrays to hold the deck of cards, player hand, computer hand, and community cards
    static string[] deck = new string[52];
    static string[] playerHand = new string[2];
    static string[] computerHand = new string[2];
    static string[] communityCards = new string[5];
    
    // Index for dealing cards from the deck
    static int currentCardIndex = 0;
    
    // Player and computer stacks, blinds, pot, and current bets
    static int playerStack = 200;
    static int computerStack = 200;
    static int smallBlind = 1;
    static int bigBlind = 2;
    static int pot = 0;
    static int playerBet = 0;
    static int computerBet = 0;
    
    // Flag to determine who is the small blind
    static bool isPlayerSmallBlind = true;
    
    // File path for saving and loading game state (not yet implemented)
    static string gameFilePath = "game_data.json";

    static void Main()
{
    bool isLoadingGame = false; // Flag to indicate if a game is being loaded
    
    // Ensure the game data file exists or create it
    if (!File.Exists(gameFilePath))
    {
        File.WriteAllText(gameFilePath, "{}");
    }
    
    int input;
    do
    {
        Console.Clear();
        // Display the main menu options
        Console.WriteLine("Welcome to The Poker App! Please choose one of the following options:");
        Console.WriteLine("1. Start new game");
        Console.WriteLine("2. Load game");
        Console.WriteLine("3. Erase game");
        Console.WriteLine("4. View games");
        Console.WriteLine("5. Quit");
        
        // Read user input and convert to integer
        input = Convert.ToInt32(Console.ReadLine());
        
        // Process the user's choice
        switch (input)
        {
            case 1:
                isLoadingGame = false;
                StartNewGame(); // Start a new poker game
                break;
            case 2:
                isLoadingGame = true;
                LoadGame(); // Load the saved game
                break;
            case 3:
                EraseGame();
                break;
            case 4:
                break;
            case 5:
                Console.WriteLine("Quitting...");
                break;
            default:
                // Handle invalid menu options
                Console.WriteLine("Invalid option, please try again");
                WaitForUserInput();
                break;
        }

        if (isLoadingGame)
        {
            // Ensure that the loaded game continues to the next hand
            while (playerStack > 0 && computerStack > 0)
            {
                Console.Clear();
                if (!ContinueGame())
                {
                    break;
                }
            }

            // Game over message when the loop exits
            Console.WriteLine("Game Over");
            Console.WriteLine($"Final Player Stack: {playerStack}");
            Console.WriteLine($"Final Computer Stack: {computerStack}");
            WaitForUserInput();
        }
    } while (input != 5); // Continue until the user chooses to quit
}



    static void StartNewGame()
    {
        // Main game loop
        while (playerStack > 0 && computerStack > 0)
        {
            Console.Clear();
            // Initialize game state for a new hand

            InitializeDeck();
            ShuffleDeck();
            currentCardIndex = 0;
            pot = 0;
            playerBet = 0;
            computerBet = 0;

            // Deal initial hands and handle blinds
            Console.Clear();
            DealInitialCards();
            HandleBlinds();
            Console.WriteLine("Preflop: No community cards yet.");
            DisplayPlayerHand();
            DisplayPot();
            DisplayStacks();

            bool handEnded = false;

            // Betting round before the flop
            if (isPlayerSmallBlind)
            {
                handEnded = !BettingRound(true, true); // Player starts preflop
            }
            else
            {
                handEnded = !BettingRound(false, true); // Computer starts preflop
            }

            if (handEnded)
            {
                EndHand();
                continue;
            }

            // Flop: Deal three community cards
            Console.WriteLine("Flop:");
            DealCommunityCards(1);
            DisplayCommunityCards();
            DisplayPot();
            DisplayStacks();
            if (!isPlayerSmallBlind)
            {
                handEnded = !BettingRound(true, false); // player starts postflop
            }
            else
            {
                handEnded = !BettingRound(false, false); // computer starts postflop

            }

            if (handEnded)
            {
                EndHand();
                continue;
            }

            // Turn: Deal one community card
            Console.WriteLine("Turn:");
            DealCommunityCards(2); // Deal the turn card
            DisplayCommunityCards();
            DisplayPot();
            DisplayStacks();

            if (!isPlayerSmallBlind)
            {
                handEnded = !BettingRound(true, false); // player starts postflop
            }
            else
            {
                handEnded = !BettingRound(false, false); // copmuter starts postflop

            }

            if (handEnded)
            {
                EndHand();
                continue;
            }

            // River: Deal the final community card
            Console.WriteLine("River:");
            DealCommunityCards(3); // Deal the river card
            DisplayCommunityCards();
            DisplayPot();
            DisplayStacks();

            if (!isPlayerSmallBlind)
            {
                handEnded = !BettingRound(true, false); // player starts postflop
            }
            else
            {
                handEnded = !BettingRound(false, false); // computer starts postflop

            }

            if (handEnded)
            {
                EndHand();
                continue;
            }

            Console.WriteLine($"Computer hand: {computerHand[0]} {computerHand[1]}");
            // Determine the winner of the hand

            DetermineWinner();
            EndHand();
        }

        // Game over message when the loop exits
        Console.WriteLine("Game Over");
        Console.WriteLine($"Final Player Stack: {playerStack}");
        Console.WriteLine($"Final Computer Stack: {computerStack}");
        WaitForUserInput();
    }

    // Handles a single betting round
    static bool BettingRound(bool playerStarts, bool isPreflop)
    {
        bool roundOver = false;

        while (!roundOver)
        {
            if (playerStarts)
            {
                // Player action
                if (!PlayerAction(isPreflop)) return false; // Player folded
                if (playerBet == computerBet) break;

                // Computer action
                if (!ComputerAction()) return false; // Computer folded
                if (playerBet == computerBet) break;
            }
            else
            {
                // Computer action
                if (!ComputerAction()) return false; // Computer folded
                if (playerBet == computerBet) break;

                // Player action
                if (!PlayerAction(isPreflop)) return false; // Player folded
                if (playerBet == computerBet) break;
            }
        }

        return true; // Return true if the round completed
    }

    static void EndHand()
{
    // Display end-of-hand summary
    Console.WriteLine("Hand Over");
    Console.WriteLine($"Pot: {pot}");
    Console.WriteLine($"Player Stack: {playerStack}");
    Console.WriteLine($"Computer Stack: {computerStack}");
        // Save game data to file
    SaveGameData();

    // Clear the community cards for the next hand
    Array.Clear(communityCards, 0, communityCards.Length);
    
    // Reset bets and pot for the next hand
    playerBet = 0;
    computerBet = 0;
    pot = 0;
    
    // Switch blinds for the next hand
    isPlayerSmallBlind = !isPlayerSmallBlind;



    WaitForUserInput();
}


    // Initialize a standard deck of 52 cards
    static void InitializeDeck()
    {
        string[] suits = { "\u2664", "\u2661", "\u2662", "\u2667" }; // Hearts, Diamonds, Clubs, Spades
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

    // Shuffle the deck of cards using the Fisher-Yates algorithm
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

    // Deal the next card from the deck
    static string DealCard()
    {
        return deck[currentCardIndex++];
    }

    // Deal the initial two cards to both the player and the computer
    static void DealInitialCards()
    {
        playerHand[0] = DealCard();
        playerHand[1] = DealCard();
        computerHand[0] = DealCard();
        computerHand[1] = DealCard();
    }

    // Deal community cards based on the stage of the hand
    static void DealCommunityCards(int stage)
    {
        switch (stage)
        {
            case 1: // Flop: 3 cards
                communityCards[0] = DealCard();
                communityCards[1] = DealCard();
                communityCards[2] = DealCard();
                break;
            case 2: // Turn: 1 card
                communityCards[3] = DealCard();
                break;
            case 3: // River: 1 card
                communityCards[4] = DealCard();
                break;
        }
    }

    // Handle the posting of blinds based on whether the player or computer is the small blind
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

    // Handle player actions during their turn
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

                // Ensure bet/raise amount is valid
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

    // Handle computer actions during its turn
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

    // Determine the winner of the hand 
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
    // Evaluate the hands
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

    Array.Sort(cardValues, (a, b) => GetCardValue(b).CompareTo(GetCardValue(a))); // Sort values in descending order
    Array.Sort(cardSuits); // Sort suits

    if (IsRoyalFlush(cardValues, cardSuits)) { highCard = GetCardValue(cardValues[0]); return 10; }
    if (IsStraightFlush(cardValues, cardSuits)) { highCard = GetCardValue(cardValues[0]); return 9; }
    if (IsFourOfAKind(cardValues)) { highCard = GetCardValue(cardValues[0]); return 8; }
    if (IsFullHouse(cardValues)) { highCard = GetCardValue(cardValues[0]); return 7; }
    if (IsFlush(cardSuits)) { highCard = GetCardValue(cardValues[0]); return 6; }
    if (IsStraight(cardValues)) { highCard = GetCardValue(cardValues[0]); return 5; }
    if (IsThreeOfAKind(cardValues)) { highCard = GetCardValue(cardValues[0]); return 4; }
    if (IsTwoPair(cardValues, out highCard)) { return 3; }
    if (IsOnePair(cardValues, out highCard)) { return 2; }

    highCard = GetCardValue(cardValues[0]); // High card
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
            i++; // Skip the next card to avoid counting the same pair twice
        }
    }

    if (pairs.Count >= 2)
    {
        pairs.Sort();
        highCard = pairs.Last(); // Highest pair
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

    // Display the community cards
    static void DisplayCommunityCards()
    {
        Console.WriteLine("Community cards: " + string.Join(" ", communityCards.Where(card => card != null)));
    }

    // Display the current pot amount
    static void DisplayPot()
    {
        Console.WriteLine($"Pot: {pot}");
    }

    // Display the stacks of both the player and the computer
    static void DisplayStacks()
    {
        Console.WriteLine($"Your stack: {playerStack}");
        Console.WriteLine($"Computer stack: {computerStack}");
    }

    // Pause execution until the user presses a key
    static void WaitForUserInput()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
    static void SaveGameData()
{
    // Create the new hand data
    var handData = new
    {
        PlayerStack = playerStack,
        ComputerStack = computerStack,
        SmallBlind = smallBlind,
        BigBlind = bigBlind,
        Pot = pot,
        PlayerBet = playerBet,
        ComputerBet = computerBet,
        PlayerHand = playerHand,
        ComputerHand = computerHand,
        CommunityCards = communityCards
    };

    // Check if the game data file exists
    if (File.Exists(gameFilePath))
    {
        // Load existing game data
        var existingData = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(gameFilePath));
        
        // Add new hand data to the history
        var history = existingData!.ContainsKey("HandHistory") 
            ? JsonConvert.DeserializeObject<List<object>>(existingData["HandHistory"].ToString()!) 
            : new List<object>();

        history!.Add(handData);

        // Update existing data with new history
        existingData["HandHistory"] = history;

        // Write updated data back to file
        string updatedJson = JsonConvert.SerializeObject(existingData, Formatting.Indented);
        File.WriteAllText(gameFilePath, updatedJson);
    }
    else
    {
        // Create new game data with hand history
        var newData = new
        {
            HandHistory = new List<object> { handData }
        };

        string json = JsonConvert.SerializeObject(newData, Formatting.Indented);
        File.WriteAllText(gameFilePath, json);
    }
}
static void LoadGame()
{
    if (File.Exists(gameFilePath))
    {
        // Load the existing game data
        var existingData = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(gameFilePath));

        if (existingData != null && existingData.ContainsKey("HandHistory"))
        {
            var history = JsonConvert.DeserializeObject<List<object>>(existingData["HandHistory"].ToString());

            if (history != null && history.Count > 0)
            {
                // Load the most recent hand
                var lastHand = history[history.Count - 1];
                var handData = JsonConvert.DeserializeObject<Dictionary<string, object>>(lastHand.ToString());

                if (handData != null)
                {
                    playerStack = Convert.ToInt32(handData["PlayerStack"]);
                    computerStack = Convert.ToInt32(handData["ComputerStack"]);
                    smallBlind = Convert.ToInt32(handData["SmallBlind"]);
                    bigBlind = Convert.ToInt32(handData["BigBlind"]);
                    pot = Convert.ToInt32(handData["Pot"]);
                    playerBet = Convert.ToInt32(handData["PlayerBet"]);
                    computerBet = Convert.ToInt32(handData["ComputerBet"]);
                    playerHand = JsonConvert.DeserializeObject<string[]>(handData["PlayerHand"].ToString());
                    computerHand = JsonConvert.DeserializeObject<string[]>(handData["ComputerHand"].ToString());
                    communityCards = JsonConvert.DeserializeObject<string[]>(handData["CommunityCards"].ToString());

                    // Print loaded game state
                    Console.WriteLine("Game loaded successfully!");
                    DisplayPlayerHand();
                    DisplayCommunityCards();
                    DisplayPot();
                    DisplayStacks();
                }
                else
                {
                    Console.WriteLine("No saved game data found.");
                }
            }
            else
            {
                Console.WriteLine("No hand history found in the saved game.");
            }
        }
        else
        {
            Console.WriteLine("No hand history found in the saved game.");
        }
    }
    else
    {
        Console.WriteLine("No saved game data file found.");
    }
    WaitForUserInput();
}

static bool ContinueGame()
{
    // Main game loop for continuing the current hand
    if (playerStack <= 0 || computerStack <= 0)
    {
        return false; // End the game if either player or computer is out of chips
    }

    // Continue the game based on the current stage
    Console.Clear();
    
    // Assuming the game has progressed and you need to handle the remaining stages.
    // The state is restored so you should skip the initial setup and jump to the appropriate stage.
    // Here’s a placeholder for your game stages handling logic.
    
    // Example placeholder logic:
    if (communityCards[4] == null) // If River is not dealt
    {
        Console.WriteLine("River:");
        DealCommunityCards(3); // Deal the river card
        DisplayCommunityCards();
        DisplayPot();
        DisplayStacks();

        // Simulate betting round
        if (!BettingRound(true, false)) // Continue with the player action
        {
            return false; // Hand ended if the player folded
        }

        // Determine winner
        DetermineWinner();
    }
    else if (communityCards[3] == null) // If Turn is not dealt
    {
        Console.WriteLine("Turn:");
        DealCommunityCards(2); // Deal the turn card
        DisplayCommunityCards();
        DisplayPot();
        DisplayStacks();

        // Simulate betting round
        if (!BettingRound(true, false)) // Continue with the player action
        {
            return false; // Hand ended if the player folded
        }

        // Continue to River if necessary
    }
    else if (communityCards[2] == null) // If Flop is not dealt
    {
        Console.WriteLine("Flop:");
        DealCommunityCards(1); // Deal the flop cards
        DisplayCommunityCards();
        DisplayPot();
        DisplayStacks();

        // Simulate betting round
        if (!BettingRound(true, false)) // Continue with the player action
        {
            return false; // Hand ended if the player folded
        }

        // Continue to Turn if necessary
    }
    else // Initial deal and blinds handling
    {
        // The initial dealing of cards and blinds should already be handled by the game setup
        // Just show the state and continue with betting if required
    }
    
    return true; // Continue if the hand was not ended
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



}
