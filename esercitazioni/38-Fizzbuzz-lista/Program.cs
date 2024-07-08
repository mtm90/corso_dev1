List<int> hundred = [1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90,91,92,93,94,95,96,97,98,99,100];
        List<int> Fizz = [];
        List<int> Buzz = [];
        List<int> FizzBuzz = []; 
        Random random = new Random();
        List<int> usedIndexes = [];

        while (usedIndexes.Count != hundred.Count)
        {
            int index = random.Next(0, hundred.Count);
            if (!usedIndexes.Contains(index))
            {
                usedIndexes.Add(index);
                int numero = hundred[index];
                if (numero % 3 == 0 && numero % 5 == 0)
                {
                    FizzBuzz.Add(numero);
                }
                else if (numero % 3 == 0)
                {
                    Fizz.Add(numero);
                }
                else if (numero % 5 == 0)
                {
                    Buzz.Add(numero);
                }
            }
        }
        // ordino gli elementi della lista
        FizzBuzz.Sort();
        Fizz.Sort();
        Buzz.Sort();
        Console.WriteLine("Fizz:");
        foreach (int i in Fizz)
        {
            Console.WriteLine(i);
        }
        Console.WriteLine("Buzz:");
        foreach (int i in Buzz)
        {
            Console.WriteLine(i);
        }
        Console.WriteLine("FizzBuzz:");
        foreach (int i in FizzBuzz)
        {
            Console.WriteLine(i);
        }


        // FizzBuzz = FizzBuzz.Distinct().ToList();

        // togli gli elementi duplicati dalla lista