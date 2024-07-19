
class Program
{
    static string[] deck = new string[52];
    static string[] playerHand = new string[2];
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
                handEnded = !PlayerAction() || !ComputerAction();
            }
            else
            {
                handEnded = !ComputerAction() || !PlayerAction();
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

            if (isPlayerSmallBlind)
            {
                handEnded = !PlayerAction() || !ComputerAction();
            }
            else
            {
                handEnded = !ComputerAction() || !PlayerAction();
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

            if (isPlayerSmallBlind)
            {
                handEnded = !PlayerAction() || !ComputerAction();
            }
            else
            {
                handEnded = !ComputerAction() || !PlayerAction();
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

            if (isPlayerSmallBlind)
            {
                handEnded = !PlayerAction() || !ComputerAction();
            }
            else
            {
                handEnded = !ComputerAction() || !PlayerAction();
            }

            if (handEnded)
            {
                EndHand();
                continue;
            }

            DetermineWinner();
            EndHand();
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
        string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
        string[] values = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

        int index = 0;
        for (int i = 0; i < suits.Length; i++)
        {
            for (int j = 0; j < values.Length; j++)
            {
                deck[index] = $"{values[j]} of {suits[i]}";
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

        int callAmount = isPlayerSmallBlind ? bigBlind - playerBet : bigBlind - playerBet;

        switch (action)
        {
            case 1: // Bet
                Console.WriteLine("Enter your bet amount:");
                int betAmount = Convert.ToInt32(Console.ReadLine());
                if (betAmount > playerStack)
                {
                    Console.WriteLine("Insufficient funds, betting all-in.");
                    betAmount = playerStack;
                }
                playerStack -= betAmount;
                pot += betAmount;
                playerBet = betAmount;
                break;
            case 2: // Raise
                Console.WriteLine("Enter your raise amount:");
                int raiseAmount = Convert.ToInt32(Console.ReadLine());
                if (raiseAmount > playerStack)
                {
                    Console.WriteLine("Insufficient funds, raising all-in.");
                    raiseAmount = playerStack;
                }
                playerStack -= raiseAmount;
                pot += raiseAmount;
                playerBet += raiseAmount;
                break;
            case 3: // Call
                if (callAmount > playerStack)
                {
                    Console.WriteLine("Insufficient funds, calling all-in.");
                    callAmount = playerStack;
                }
                playerStack -= callAmount;
                pot += callAmount;
                playerBet = bigBlind;
                break;
            case 4: // Check
                if (playerBet == bigBlind)
                {
                    Console.WriteLine("Checked.");
                }
                else
                {
                    Console.WriteLine("Cannot check; bet required.");
                    PlayerAction();
                }
                break;
            case 5: // Fold
                Console.WriteLine("You folded.");
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
        // Simple computer logic (for demonstration purposes)
        Random rand = new Random();
        int action = rand.Next(1, 6);
        int callAmount = isPlayerSmallBlind ? bigBlind - computerBet : bigBlind - computerBet;

        switch (action)
        {
            case 1: // Bet
                int betAmount = rand.Next(1, computerStack + 1);
                computerStack -= betAmount;
                pot += betAmount;
                computerBet = betAmount;
                break;
            case 2: // Raise
                int raiseAmount = rand.Next(1, computerStack + 1);
                computerStack -= raiseAmount;
                pot += raiseAmount;
                computerBet += raiseAmount;
                break;
            case 3: // Call
                if (callAmount > computerStack)
                {
                    callAmount = computerStack;
                }
                computerStack -= callAmount;
                pot += callAmount;
                computerBet = bigBlind;
                break;
            case 4: // Check
                if (computerBet == bigBlind)
                {
                    Console.WriteLine("Computer checks.");
                }
                else
                {
                    Console.WriteLine("Computer cannot check; bet required.");
                    ComputerAction();
                }
                break;
            case 5: // Fold
                Console.WriteLine("Computer folded.");
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
        // For simplicity, randomly choose the winner. 
        // A full implementation should evaluate the best hand between player and computer
        Random rand = new Random();
        int winner = rand.Next(1, 3); // 1 for Player, 2 for Computer
        if (winner == 1)
        {
            Console.WriteLine("Player wins!");
            playerStack += pot;
        }
        else
        {
            Console.WriteLine("Computer wins!");
            computerStack += pot;
        }
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
