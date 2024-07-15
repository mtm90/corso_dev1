using Spectre.Console;

Console.Clear();
        List<string> members = new List<string> { "Mattia", "Allison", "Silvano", "Ginevra", "Daniele", "Matteo", "Francesco", "Serghej" };
        string path = @"members.txt";
            // Check if the file exists and is not empty, then load members from the file
        if (File.Exists(path) && File.ReadAllText(path).Length != 0)
        {
            string[] savedMembers = File.ReadAllLines(path);
            members = new List<string>(savedMembers);
        }
        else
        {
            // If the file does not exist or is empty, write the initial members to the file
            File.WriteAllLines(path, members);
        }

        bool programIsRunning = true;

            // Main program loop
        while (programIsRunning)
        {
            AnsiConsole.WriteLine("");
            var input = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold]Members menu[/]")
                    .PageSize(9)
                    .AddChoices(new[]
                    {
                        "add member", "show members", "order members",
                        "find members", "delete members", "edit members",
                        "sort in teams", "sort in teams with GetRange", "Quit"
                    }));

            // Handle the user's menu selection
            switch (input)
            {
                case "add member":
                    Console.Clear();
                    AnsiConsole.WriteLine("Enter new member name");
                    string newMember = Console.ReadLine()!;
                    if (members.Contains(newMember))
                    {
                        Console.WriteLine("The name is already in the list, try a different one");
                    }
                    else
                    {
                        members.Add(newMember);
                        File.AppendAllText(path, newMember + "\n");
                    }
                    break;

                case "show members":
                    Console.Clear();
                    var table = new Table();
                    table.AddColumn("[bold]Members[/]");
                    foreach (var member in members)
                    {
                        table.AddRow(member);
                    }
                    AnsiConsole.Write(table);
                    Console.WriteLine($"Total members: {members.Count}");
                    break;

                case "order members":
                    Console.Clear();
                    string sortInput = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Sort Menu")
                            .PageSize(3)
                            .AddChoices(new[] {
                                "alphabetical order", "alphabetical order inverted",
                                "go back"
                            }));
                    switch (sortInput)
                    {
                        case "alphabetical order":
                            members.Sort();
                            break;
                        case "alphabetical order inverted":
                            members.Sort();
                            members.Reverse();
                            break;
                        case "go back":
                            break;
                    }
                    break;

                case "find members":
                    Console.WriteLine("Insert name:");
                    string name = Console.ReadLine()!;
                    if (members.Contains(name))
                    {
                        Console.WriteLine("The name is listed");
                    }
                    else
                    {
                        Console.WriteLine("The name is not listed");
                    }
                    break;

                case "delete members":
                    Console.WriteLine("Type in the name of the member you want to delete");
                    string memberToDelete = Console.ReadLine()!;
                    if (members.Contains(memberToDelete))
                    {
                        members.Remove(memberToDelete);
                        Console.WriteLine("The name was deleted");
                        File.WriteAllLines(path, members);
                    }
                    else
                    {
                        Console.WriteLine("The name is not listed");
                    }
                    break;

                case "edit members":
                    Console.WriteLine("Member name:");
                    name = Console.ReadLine()!;
                    if (members.Contains(name))
                    {
                        Console.WriteLine("New name: ");
                        string newName = Console.ReadLine()!;
                        int index = members.IndexOf(name);
                        members[index] = newName;
                        Console.WriteLine("The member was successfully edited");
                        File.WriteAllLines(path, members);
                    }
                    else
                    {
                        Console.WriteLine("The member is not in the list");
                    }
                    break;

                case "sort in teams":
                    Console.Clear();
                    Random mix = new();
                    bool addToteam1 = true;
                    var team1Table = new Table().AddColumn("[bold]Team 1[/]");
                    var team2Table = new Table().AddColumn("[bold]Team 2[/]");

                    // Randomly distribute members into two teams
                    while (members.Count > 0)
                    {
                        int randomIndex = mix.Next(0, members.Count);
                        if (addToteam1)
                        {
                            team1Table.AddRow(members[randomIndex]);
                            addToteam1 = false;
                        }
                        else
                        {
                            team2Table.AddRow(members[randomIndex]);
                            addToteam1 = true;
                        }
                        members.RemoveAt(randomIndex);
                    }
                    AnsiConsole.Write(team1Table);
                    AnsiConsole.Write(team2Table);

                    // Reload members from the file after sorting into teams
                    members.AddRange(File.ReadAllLines(path));
                    break;

                case "sort in teams with GetRange":
                    Console.Clear();
                    int split = members.Count / 2;
                    List<string> team1 = members.GetRange(0, split);
                    List<string> team2 = members.GetRange(split, members.Count - split);
                    var team1TableWithGetRange = new Table().AddColumn("[bold]Team 1[/]");
                    var team2TableWithGetRange = new Table().AddColumn("[bold]Team 2[/]");

                    // Display teams
                    foreach (string member in team1)
                    {
                        team1TableWithGetRange.AddRow(member);
                    }
                    foreach (string member in team2)
                    {
                        team2TableWithGetRange.AddRow(member);
                    }
                    AnsiConsole.Write(team1TableWithGetRange);
                    AnsiConsole.Write(team2TableWithGetRange);
                    break;

                case "Quit":
                    programIsRunning = false;
                    break;
            }
        }

        // Save the members list to the file before exiting
        File.WriteAllLines(path, members);