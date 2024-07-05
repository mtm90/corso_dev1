Console.Clear();
int min = 1;
int max = 100;
int computerGuess;
int guesses = 0;
bool gameIsRunning = true;
Console.WriteLine("Think of a number between 1 and 100, i'm gonna try and guess it.");
Thread.Sleep(2500);

    while (gameIsRunning)
    {
        Console.Clear();
        Console.WriteLine($"Number of guesses so far: {guesses}");
        computerGuess = (min + max)/ 2;
        Console.WriteLine($"Is your number {computerGuess}(c), higher(+) or lower(-)?");
        char answer = Console.ReadKey(true).KeyChar;
        guesses++;
        if (guesses == 5)
        {
            Console.Clear();
            Console.WriteLine($"Number of guesses so far: {guesses}");
            Console.WriteLine("Damn it, i lost!! Well played...");
            gameIsRunning = false;
        }
        if (answer == 'c')
            {
                Console.WriteLine("HAHA! I won!");
                Console.WriteLine($"Number of guesses: {guesses}");
                gameIsRunning = false;
            }
        else if (answer == '+')
            {
                min = computerGuess + 1;
            }
        else if (answer == '-')
            {
                max = computerGuess - 1;
            }
        if (answer != 'c' && answer != '+' && answer != '-')
        {
            Console.WriteLine("Please type in a valid character");
            Thread.Sleep(1300);
            guesses--;
        }
    }