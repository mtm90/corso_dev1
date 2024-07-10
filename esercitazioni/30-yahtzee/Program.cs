Console.Clear();
        Console.WriteLine("Let's play Yahtzee! Press a key to throw 5 dice");
        Console.ReadKey(true);
        Random random = new Random();
        int[] dadi = new int[5];
        
        for (int i = 0; i < dadi.Length; i++)
        {
            dadi[i] = random.Next(1, 7);
        }
        Console.Write("Initial roll: ");
        foreach (int i in dadi)
        {
            Console.Write($"{i} ");
        }
        Console.WriteLine("");
        bool keepPlaying = true;
        int numberOfThrows = 0;
        while (keepPlaying)
        {
            Console.WriteLine("Do you want to reroll any dice? (yes/no)");
            string answer = Console.ReadLine()!;            
            if (answer == "yes")
            {
                Console.WriteLine("Enter the indices (1-5) of the dice you want to reroll, separated by spaces:");
                string indicesInput = Console.ReadLine()!;
                string[] indices = indicesInput.Split(' ');
                foreach (string index in indices)
                {
                    int diceIndex = Convert.ToInt32(index);
                    if (diceIndex >= 1 && diceIndex <= 5)
                    {
                        dadi[diceIndex - 1] = random.Next(1, 7);
                    }
                    else
                    {
                        Console.WriteLine($"Invalid index: {index}. Please enter a number between 1 and 5.");
                    }
                }
                Console.Write("New roll: ");
                foreach (int i in dadi)
                {
                    Console.Write($"{i} ");
                }
                Console.WriteLine("");
                numberOfThrows++;
                Console.WriteLine($"number of throws : {numberOfThrows}");
            }
            else if (answer == "no")
            {
                Console.WriteLine("These are your final dice:");
                foreach (int i in dadi)
                {
                    Console.Write($"{i} ");
                }
                Console.WriteLine("");
                Console.WriteLine($"It took you {numberOfThrows} tries to make a Yahtzee!");
                if (numberOfThrows < 5)
                {
                    Console.WriteLine("Hot Damn... you're a pro!");
                }
                else if (numberOfThrows > 5 && numberOfThrows < 8)
                {
                    Console.WriteLine("You're good but u can do better!");    
                }
                else if (numberOfThrows > 8 && numberOfThrows <12)
                {
                    Console.WriteLine("You're kinda trash!");  
                }
                else if (numberOfThrows > 12)
                {
                    Console.WriteLine("You're awful, try another game this is not for you");  
                }
                keepPlaying = false;
            }
            
        }