Console.WriteLine("Throw 5 dices");
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
    Console.WriteLine("Which dice do u wanna reroll?");
    Console.ReadLine();
    




