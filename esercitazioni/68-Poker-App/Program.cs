using Newtonsoft.Json;
using System;
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
            DisplayStacks();

            bool handEnded = false;

            // Preflop Betting Round: Player acts first if small blind, otherwise computer
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

            // Flop
            Console.WriteLine("Flop:");
            DealCommunityCards(1);
            DisplayCommunityCards();
            DisplayPot();
            DisplayStacks();

            handEnded = !BettingRound(false, false); // Computer starts postflop

            if (handEnded)
            {
                EndHand();
                continue;
            }

            // Turn
            Console.WriteLine("Turn:");
            DealCommunityCards(2); // Deal the turn card
            DisplayCommunityCards();
            DisplayPot();
            DisplayStacks();

            handEnded = !BettingRound(false, false); // Computer starts postturn

            if (handEnded)
            {
                EndHand();
                continue;
            }

            // River
            Console.WriteLine("River:");
            DealCommunityCards(3); // Deal the river card
            DisplayCommunityCards();
            DisplayPot();
            DisplayStacks();

            handEnded = !BettingRound(false, false); // Computer starts postriver

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

    static bool BettingRound(bool playerStarts, bool isPreflop)
    {
        bool roundOver = false;

        while (!roundOver)
        {
            if (playerStarts)
            {
                if (!PlayerAction(isPreflop)) return false; // Player folded
                if (playerBet == computerBet) break;

                if (!ComputerAction()) return false; // Computer folded
                if (playerBet == computerBet) break;
            }
            else
            {
                if (!ComputerAction()) return false; // Computer folded
                if (playerBet == computerBet) break;

                if (!PlayerAction(isPreflop)) return false; // Player folded
                if (playerBet == computerBet) break;
            }
        }

        return true;
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
            case 2: // Turn: 1 card
                communityCards[3] = DealCard();
                break;
            case 3: // River: 1 card
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
                    amount = playerStack;
                }
                playerStack -= amount;
                pot += amount;
                playerBet += amount;
                Console.WriteLine($"You bet/raised {amount}");
                break;

            case 2: // Call
                if (callAmount > playerStack)
                {
                    callAmount = playerStack;
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
                return false;

            default:
                Console.WriteLine("Invalid action. Try again.");
                return PlayerAction(isPreflop); // Retry if the action is invalid
        }
        return true;
    }

    static bool ComputerAction()
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
                    callAmount = computerStack;
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
                return false;

            default:
                Console.WriteLine("Computer takes an invalid action.");
                return ComputerAction(); // Retry if action is invalid
        }
        return true;
    }

    static void DetermineWinner()
    {
        // Placeholder for determining the winner logic
        Console.WriteLine("Determining the winner...");
        // This is a placeholder implementation. Replace with proper hand evaluation logic.

        // Example winner determination (randomly for now):
        Random rand = new Random();
        bool playerWins = rand.Next(0, 2) == 0;

        if (playerWins)
        {
            Console.WriteLine("Player wins the pot!");
            playerStack += pot;
        }
        else
        {
            Console.WriteLine("Computer wins the pot!");
            computerStack += pot;
        }
        pot = 0; // Reset the pot for the next hand
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
}
