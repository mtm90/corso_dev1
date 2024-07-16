using Spectre.Console;

Console.Clear();
List<string> members = new List<string> { "Mattia", "Allison", "Unknown", "Ginevra", "Daniele", "Matteo", "Francesco", "Serghej" };
string path = @"members.txt";

// Load members from the file
LoadMembers();

bool programIsRunning = true;

// Main program loop
while (programIsRunning)
{ 
    var highlightStyle = new Style().Foreground(Color.LightGoldenrod1);
    Console.Clear();
    AnsiConsole.WriteLine("");
    var input = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("[bold LightGoldenrod1]Members menu[/]")
            .PageSize(9)
            .HighlightStyle(highlightStyle)
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
            AddMember();
            break;

        case "show members":
            ShowMembers();
            break;

        case "order members":
            OrderMembers();
            break;

        case "find members":
            FindMembers();
            break;

        case "delete members":
            DeleteMember();
            break;

        case "edit members":
            EditMember();
            break;

        case "sort in teams":
            SortInTeams();
            break;

        case "sort in teams with GetRange":
            SortInTeamsWithGetRange();
            break;

        case "Quit":
            programIsRunning = false;
            break;
    }
}
// Save the members list to the file before exiting
SaveMembers();

void SaveMembers()
{
    File.WriteAllLines(path, members);
}

void AddMember()
{
    Console.Clear();
    AnsiConsole.WriteLine("Enter new member name");
    string newMember = Console.ReadLine()!;
    if (members.Contains(newMember))
    {
        AnsiConsole.WriteLine($"{newMember} is already in the list, try a different one");
        Thread.Sleep(1000);
    }
    else
    {
        members.Add(newMember);
        File.AppendAllText(path, newMember + "\n");
        AnsiConsole.WriteLine($"{newMember} was added to the Members Menu");
        Thread.Sleep(1000);
    }
}

void ShowMembers()
{
    Console.Clear();
    var table = new Table();
    table.AddColumn("[green bold]Members[/]");
    foreach (var member in members)
    {
        table.AddRow(member);
    }
    AnsiConsole.Write(table);
    AnsiConsole.MarkupLine($"[green bold]Total members: {members.Count}[/]");
    AnsiConsole.WriteLine("Press any key to continue");
    Console.ReadKey(true);
}

void OrderMembers()
{
    Console.Clear();
    var highlightStyle = new Style().Foreground(Color.Violet);
    string sortInput = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("[bold Violet]Order Menu[/]")
            .PageSize(3)
            .HighlightStyle(highlightStyle)
            .AddChoices(new[] {
                "alphabetical order", "alphabetical order inverted",
                "go back"
            }));
    switch (sortInput)
    {
        case "alphabetical order":
            members.Sort();
            AnsiConsole.WriteLine("Memmbers order changed");
            Thread.Sleep(1000);
            break;
        case "alphabetical order inverted":
            members.Sort();
            members.Reverse();
            AnsiConsole.WriteLine("Memmbers order changed");
            Thread.Sleep(1000);
            break;
        case "go back":
            break;
    }
}

void FindMembers()
{
    AnsiConsole.WriteLine("Insert name:");
    string name = Console.ReadLine()!;
    if (members.Contains(name))
    {
        AnsiConsole.WriteLine("The name is listed");
        Thread.Sleep(1000);
    }
    else
    {
        AnsiConsole.WriteLine("The name is not listed");
        Thread.Sleep(1000);
    }
}

void DeleteMember()
{
    var highlightStyle = new Style().Foreground(Color.Red);
    var deleteInput = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[red bold]delete menu[/]")
                .PageSize(members.Count)
                .HighlightStyle(highlightStyle)
                .AddChoices(
                    members
                ));
                

    members.Remove(deleteInput);
    AnsiConsole.WriteLine($"{deleteInput} was deleted");
    Thread.Sleep(1000);
}

void EditMember()
{
    var highlightStyle = new Style().Foreground(Color.Yellow);
    var editInput = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold Yellow]Edit menu[/]")
                .PageSize(members.Count)
                .HighlightStyle(highlightStyle)
                .AddChoices(
                    members
                ));
    AnsiConsole.WriteLine("Type in edited name:");
    string newName = Console.ReadLine()!;
    int index = members.IndexOf(editInput);
    members[index] = newName;
    AnsiConsole.WriteLine("The name was succesfully edited");
    Thread.Sleep(1000);
    SaveMembers();
}

void SortInTeams()
{
    Console.Clear();
    Random mix = new();
    bool addToteam1 = true;
    var team1Table = new Table().AddColumn("[red bold]Team 1[/]");
    var team2Table = new Table().AddColumn("[blue bold]Team 2[/]");
    team1Table.Width = 15;
    team2Table.Width = 15;

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
    Thread.Sleep(1000);
    AnsiConsole.Write(team2Table);
    Thread.Sleep(1000);
    AnsiConsole.Write("");
    AnsiConsole.WriteLine("Press any key to continue");
    Console.ReadKey(true);

    // Reload members from the file after sorting into teams
    members.AddRange(File.ReadAllLines(path));
}

void SortInTeamsWithGetRange()
{
    Console.Clear();
    int split = members.Count / 2;
    List<string> team1 = members.GetRange(0, split);
    List<string> team2 = members.GetRange(split, members.Count - split);
    var team1TableWithGetRange = new Table().AddColumn("[red bold]Team 1[/]");
    var team2TableWithGetRange = new Table().AddColumn("[blue bold]Team 2[/]");
    team1TableWithGetRange.Width = 15;
    team2TableWithGetRange.Width = 15;

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
    Thread.Sleep(1000);
    AnsiConsole.Write(team2TableWithGetRange);
    Thread.Sleep(1000);
    AnsiConsole.WriteLine("Press any key to continue");
    Console.ReadKey(true);
}

void LoadMembers()
{
    if (File.Exists(path) && File.ReadAllText(path).Length != 0)
    {
        string[] savedMembers = File.ReadAllLines(path);
        members = new List<string>(savedMembers);
    }
    else
    {
        // If the file does not exist or is empty, write the initial members to the file
        SaveMembers();
    }
}
