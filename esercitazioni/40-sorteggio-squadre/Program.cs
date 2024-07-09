using Spectre.Console;

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
        List<string> team1 = [];
        List<string> team2 = [];
        Random random = new();
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

        var table1 = new Table();
        table1.AddColumn("[bold red]team 1[/]");
        foreach (string member in team1)
        {
            table1.AddRow(member);
            

        }
        AnsiConsole.Write(table1);
        var table2 = new Table();
        table2.AddColumn("[bold blue]team 2[/]");

        foreach (string member in team2)
        {
            table2.AddRow(member);

        }
        AnsiConsole.Write(table2);