class Program
{
    static string[] deck = new string[52];
    static string[] playerHand = new string[2];
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

    static void Main()
    {
        while (playerStack > 0 && computerStack > 0)
        {
            StartNewGame();
            // Swap blinds for the next hand
            isPlayerSmallBlind = !isPlayerSmallBlind;
        }
        Console.WriteLine("Game Over");
        Console.WriteLine($"Final Player Stack: {playerStack}");
        Console.WriteLine($"Final Computer Stack: {computerStack}");
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

    static void StartNewGame()
    {
        // Initialize and shuffle the deck
        InitializeDeck();
        ShuffleDeck();

        // Reset game state
        currentCardIndex = 0;
        pot = 0;
        playerBet = 0;
        computerBet = 0;

        // Deal initial cards
        DealInitialCards();

        // Handle blinds and betting
        HandleBlinds();
        Console.WriteLine("Preflop: No community cards yet.");
        DisplayPlayerHand();
        DisplayPot();

        // Betting round: Preflop
        if (isPlayerSmallBlind)
        {
            PlayerAction(); // Player is small blind
            ComputerAction(); // Computer is big blind
        }
        else
        {
            ComputerAction(); // Computer is small blind
            PlayerAction(); // Player is big blind
        }

        // Deal and display community cards stage by stage
        Console.WriteLine("Flop:");
        DealCommunityCards(1);
        DisplayCommunityCards();
        DisplayPot();
        if (isPlayerSmallBlind)
        {
            PlayerAction(); // Player is second to act
            ComputerAction(); // Computer is first to act
        }
        else
        {
            ComputerAction(); // Computer is second to act
            PlayerAction(); // Player is first to act
        }

        Console.WriteLine("Turn:");
        DealCommunityCards(2);
        DisplayCommunityCards();
        DisplayPot();
        if (isPlayerSmallBlind)
        {
            PlayerAction(); // Player is second to act
            ComputerAction(); // Computer is first to act
        }
        else
        {
            ComputerAction(); // Computer is second to act
            PlayerAction(); // Player is first to act
        }

        Console.WriteLine("River:");
        DealCommunityCards(3);
        DisplayCommunityCards();
        DisplayPot();
        if (isPlayerSmallBlind)
        {
            PlayerAction(); // Player is second to act
            ComputerAction(); // Computer is first to act
        }
        else
        {
            ComputerAction(); // Computer is second to act
            PlayerAction(); // Player is first to act
        }

        // End of the hand, determine winner
        DetermineWinner();
        Console.WriteLine("Hand Over");
        Console.WriteLine($"Pot: {pot}");
        Console.WriteLine($"Player Stack: {playerStack}");
        Console.WriteLine($"Computer Stack: {computerStack}");
        WaitForUserInput();
    }

    static void HandleBlinds()
    {
        Console.WriteLine("Blinds are posted:");
        Console.WriteLine($"Small Blind: {smallBlind}");
        Console.WriteLine($"Big Blind: {bigBlind}");

        // Deduct blinds from stacks and add to the pot
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

    static void PlayerAction()
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
                playerBet = bigBlind; // or smallBlind based on context
                break;
            case 4: // Check
                if (playerBet == bigBlind) // Can only check if no bet is required
                {
                    Console.WriteLine("Checked.");
                }
                else
                {
                    Console.WriteLine("Cannot check; bet required.");
                    PlayerAction(); // Re-prompt action
                }
                break;
            case 5: // Fold
                Console.WriteLine("You folded.");
                Environment.Exit(0); // End the game if the player folds
                break;
            default:
                Console.WriteLine("Invalid action.");
                PlayerAction(); // Re-prompt action
                break;
        }
    }

    static void ComputerAction()
    {
        Random rand = new Random();
        int action = rand.Next(1, 6); // Random action: 1=Bet, 2=Raise, 3=Call, 4=Check, 5=Fold
        int betAmount = rand.Next(0, Math.Min(computerStack, bigBlind) + 1); // Random bet between 0 and big blind

        int callAmount = bigBlind - computerBet;

        Console.WriteLine($"Computer action: {action}");
        switch (action)
        {
            case 1: // Bet
                Console.WriteLine($"Computer bets {betAmount}");
                computerStack -= betAmount;
                pot += betAmount;
                computerBet = betAmount;
                break;
            case 2: // Raise
                Console.WriteLine($"Computer raises by {betAmount}");
                computerStack -= betAmount;
                pot += betAmount;
                computerBet += betAmount;
                break;
            case 3: // Call
                if (callAmount > computerStack)
                {
                    Console.WriteLine("Computer calls all-in.");
                    callAmount = computerStack;
                }
                computerStack -= callAmount;
                pot += callAmount;
                computerBet = bigBlind; // or smallBlind based on context
                break;
            case 4: // Check
                if (computerBet == bigBlind) // Can only check if no bet is required
                {
                    Console.WriteLine("Computer checks.");
                }
                else
                {
                    Console.WriteLine("Computer cannot check; bet required.");
                    ComputerAction(); // Re-prompt action
                }
                break;
            case 5: // Fold
                Console.WriteLine("Computer folded.");
                playerStack += pot; // Player wins the pot
                pot = 0;
                break;
            default:
                Console.WriteLine("Invalid action.");
                ComputerAction(); // Re-prompt action
                break;
        }
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
}
