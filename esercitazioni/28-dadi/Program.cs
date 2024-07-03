Random random = new Random();
        int myScore = 100;
        int computerScore = 100;
        Console.WriteLine("Welcome to the dice game, you'll be playing against me!");
        Console.WriteLine($"Your Score: {myScore}");
        Console.WriteLine($"My Score: {computerScore}");
        while (myScore > 0 && computerScore > 0)
        {
            Console.WriteLine("Press a button to throw the dices");
            Console.ReadKey();
            Console.WriteLine("");
            int dice1 = random.Next(1, 7);
            int dice2 = random.Next(1, 7);
            int dice3 = random.Next(1, 7);
            int dice4 = random.Next(1, 7);
            Console.WriteLine($"Your first dice is {dice1}");
            Console.WriteLine($"Your second dice is {dice2}");
            Console.WriteLine($"My first dice is {dice3}");
            Console.WriteLine($"My second dice is {dice4}");
            int myPoints = dice1 + dice2;
            int computerPoints = dice3 + dice4;
            int pointsDifference = Math.Abs(myPoints - computerPoints);
            if (myPoints > computerPoints)
            {
                computerScore -= pointsDifference;
                if (computerScore < 0) computerScore = 0;
            }
            else if (myPoints < computerPoints)
            {
                myScore -= pointsDifference;
                if (myScore < 0) myScore = 0;
            }
            Console.WriteLine("Here are the new scores:");
            Console.WriteLine($"Your score: {myScore}");
            Console.WriteLine($"My score: {computerScore}");
        }
        if (myScore <= 0)
        {
            Console.WriteLine("Sorry, I win!");
        }
        else if (computerScore <= 0)
        {
            Console.WriteLine("Congratulations!! You Win!");
        }
    