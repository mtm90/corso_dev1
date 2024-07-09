List<string> devclass = new List<string>
        {
            "Francesco",
            "Mattia",
            "Allison",
            "Ginevra",
            "Daniele",
            "Serghej",
            "Matteo",
            "Silvano"
        };
        List<string> team1 = new List<string>();
        List<string> team2 = new List<string>();
        Random random = new Random();
        bool addToteam1 = true;
        while (devclass.Count > 0)
        {
            int randomIndex = random.Next(0, devclass.Count);
            if (addToteam1)
            {
                team1.Add(devclass[randomIndex]);
                addToteam1 = false;
            }
            else
            {
                team2.Add(devclass[randomIndex]);
                addToteam1 = true;
            }
            devclass.RemoveAt(randomIndex);
        }

        Console.WriteLine("team1:");
        foreach (string member in team1)
        {
            Console.WriteLine(member);
        }

        Console.WriteLine("team2:");
        foreach (string member in team2)
        {
            Console.WriteLine(member);
        }