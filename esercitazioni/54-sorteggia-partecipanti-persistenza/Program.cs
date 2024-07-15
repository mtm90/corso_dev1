using Spectre.Console;
Console.Clear();
List<string> members = new List<string> { "Mattia", "Allison", "Silvano", "Ginevra", "Daniele", "Matteo", "Francesco", "Serghej" };
string path = @"members.txt";
if (File.ReadAllText(path).Length != 0)
{
    string[] savedMembers = File.ReadAllLines(path);
    for (int i = 0; i < savedMembers.Length; i++)
    {
        members[i] = savedMembers[i];
    }
}
else {
for (int i = 0; i < members.Count; i++)
{
    File.AppendAllText(path, members[i] + "\n");
}
}


bool programIsRunning = true; // creo una variabile booleana che definisce quando il programma deve funzionare
while (programIsRunning)
    {
        AnsiConsole.WriteLine("");
    var input = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("[bold]Members menu[/]")
        .PageSize(9)
        .AddChoices(new[] {
            "add member", "show members", "order members", 
            "find members", "delete members", "edit members",
            "sort in teams", "sort in teams with GetRange", "Quit",
        }));  

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
                    input = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                            .Title("Sort Menu")
                            .PageSize(3)
                            .AddChoices(new[] {
                                "alphabetical order", "alphabetical order inverted",
                                "go back"
                            }));
                    switch (input)
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
                    Console.WriteLine("insert name:");
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
                    else {
                        Console.WriteLine("The name is not listed");
                    }
                    break;
                case "edit members":
                    Console.WriteLine("member name:");
                    name = Console.ReadLine()!;
                    if (members.Contains(name))
                    {
                        Console.WriteLine("New name: ");
                        string newName = Console.ReadLine()!;
                        int index = members.IndexOf(name);
                        members[index] = newName;
                        Console.WriteLine("the member was successfully edited");
                    File.ReadAllLines(path);
                    members[index] = newName;
                    File.WriteAllLines(path, members);
                    }
                    else 
                    { 
                        Console.WriteLine("the member is not in list");
                    }
                break;
                case "sort in teams":
                    Console.Clear();
                    Random mix = new();
                    bool addToteam1 = true;
                    var table2 = new Table();
                    table2.Width = 15;
                    table2.AddColumn("[bold]Team 1[/]");
                    var table3 = new Table();
                    table3.Width = 15;
                    table3.AddColumn("[bold]Team 2[/]");
                    while (members.Count > 0)
                    {
                        int randomIndex = mix.Next(0, members.Count);
                        if(addToteam1)
                        {
                            table2.AddRow(members[randomIndex]);
                            addToteam1 = false;
                        }
                        else
                        {
                            table3.AddRow(members[randomIndex]);
                            addToteam1 = true;
                        }
                        members.RemoveAt(randomIndex);
                    }
                    AnsiConsole.Write(table2);
                    AnsiConsole.Write(table3);
                    string[] names = File.ReadAllLines(path);

                    foreach (string person in names)
                    {
                        members.Add(person);
                    }

                    break;
                case "sort in teams with GetRange":
                    Console.Clear();
                    int split = members.Count/2;
                    List<string> squadra1 = members.GetRange(0, split);    
                    List<string> squadra2 = members.GetRange(split, members.Count - split);
                    var table5 = new Table();
                    table5.Width = 15;
                    table5.AddColumn("[bold]Team 1[/]");
                    var table6 = new Table();
                    table6.Width = 15;
                    table6.AddColumn("[bold]Team 2[/]");
                    foreach (string member in squadra1)
                    {
                        table5.AddRow(member);
                    }
                    foreach (string member in squadra2)
                    {
                        table6.AddRow(member);
                    }
                    AnsiConsole.Write(table5);
                    AnsiConsole.Write(table6);  
                    break;
                case "Quit":
                    programIsRunning = false;
                    break;
            }
    }
    File.WriteAllLines(path, members);