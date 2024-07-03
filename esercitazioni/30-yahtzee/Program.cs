Console.Clear();
Console.WriteLine("Let's play Yatzee! Press a key to throw 5 dices");
Console.ReadKey(true);
Random random = new Random();
int[] dadi = new int[5];
    dadi[0] = random.Next(1,7);
    dadi[1] = random.Next(1,7);
    dadi[2] = random.Next(1,7);
    dadi[3] = random.Next(1,7);
    dadi[4] = random.Next(1,7);

    foreach (int i in dadi)
    {
        Console.WriteLine(i);
    }
    bool keepPlaying = true;
        while (keepPlaying)
        {
        Console.WriteLine("Do u wanna reroll a dice?");
        string answer = Console.ReadLine()!;
        if  (answer == "yes")
            {
                Console.WriteLine("Which dice do u wanna reroll?");
                int number = Convert.ToInt32(Console.ReadLine());
                dadi[number - 1] = random.Next(1,7);
            }
        else if (answer == "no")
            {
                Console.WriteLine("These are your final dices");
                foreach (int i in dadi)
            {
            Console.WriteLine(i);
            }
            keepPlaying = false;
            }
        }

    




