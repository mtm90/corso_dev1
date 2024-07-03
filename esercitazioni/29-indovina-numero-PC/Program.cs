Console.Clear();
Random random = new Random();
        int lowerBound = 1;
        int upperBound = 100;
        int computerGuess;
        int guesses = 0;

        Console.WriteLine("Think of a number between 1 and 100, i'm gonna try and guess it.");
        Thread.Sleep(2000);
        bool gameIsRunning = true;
        while (gameIsRunning)
        {
            Console.Clear();
            computerGuess = random.Next(lowerBound, upperBound + 1); 
            Console.WriteLine($"Is your number {computerGuess}?");
            guesses++;

            string answer = Console.ReadLine()!;

            if (answer == "yes")
            {
                Console.WriteLine("HAHA! I won!");
                Console.WriteLine($"It took me {guesses} guesses");
                gameIsRunning = false;
            }
            else if (answer == "no")
            {
                Console.WriteLine($"Is it higher or lower than {computerGuess}?");
                string answer2 = Console.ReadLine()!;

                if (answer2 == "higher")
                {
                    lowerBound = computerGuess + 1;
                }
                else if (answer2 == "lower")
                {
                    upperBound = computerGuess - 1;
                }
            }
        }