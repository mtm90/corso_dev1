class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();  
        game.Play();             
    }
}

class Game
{
    public int numberToGuess { get; set; }
    public int attempts { get; set; }
    public int userGuess { get; set; }

    public Game()
    {
        Random random = new Random();
        this.numberToGuess = random.Next(1, 101);
        this.attempts = 5;
    }

    private int GetValidUserGuess()
    {
        int guess;
        string input = Console.ReadLine()!;
        
        while (!int.TryParse(input, out guess))
        {
            Console.Write("Invalid input. Please enter a valid number: ");
            input = Console.ReadLine()!;
        }

        return guess;
    }

    private void CheckGuess(int guess)
    {
        if (guess < numberToGuess)
        {
            Console.WriteLine("Too low!");
        }
        else if (guess > numberToGuess)
        {
            Console.WriteLine("Too high!");
        }
        else
        {
            Console.WriteLine("Congratulations! You've guessed the right number!");
            return;
        }
    }

    public void Play()
    
    {

        Console.WriteLine("I picked a number bewteen 1 and 100. Let's see if u can guess it in less than 5 attempts!");
        for (int i = 0; i < attempts; i++)
        {   
            
            Console.Write("Enter your guess: ");
            userGuess = GetValidUserGuess();
            
            CheckGuess(userGuess);

            if (userGuess == numberToGuess)
                return;

            Console.WriteLine($"You have {attempts - i - 1} attempts left.");
        }

        Console.WriteLine("Sorry, you've used all your attempts.");
        Console.WriteLine($"The number was: {numberToGuess}");
    }
}
