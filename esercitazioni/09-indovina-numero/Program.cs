Random random = new Random();
            int numberToGuess = random.Next(1, 101);
            int userGuess = 0;
            int attempts = 5;

            Console.WriteLine("Welcome to the Number Guessing Game!");
            Console.WriteLine("I have chosen a number between 1 and 100.");
            Console.WriteLine("You have 5 attempts to guess it. Good luck!");

            for (int i = 0; i < attempts; i++)
            {
                Console.Write("Enter your guess: ");
                string input = Console.ReadLine();
                
                // Validate if input is a number
                while (!int.TryParse(input, out userGuess))
                {
                    Console.Write("Invalid input. Please enter a valid number: ");
                    input = Console.ReadLine();
                }

                if (userGuess < numberToGuess)
                {
                    Console.WriteLine("Too low!");
                }
                else if (userGuess > numberToGuess)
                {
                    Console.WriteLine("Too high!");
                }
                else
                {
                    Console.WriteLine("Congratulations! You've guessed the right number!");
                    return;
                }

                Console.WriteLine($"You have {attempts - i - 1} attempts left.");
            }

            Console.WriteLine("Sorry, you've used all your attempts.");
            Console.WriteLine($"The number was: {numberToGuess}");
        