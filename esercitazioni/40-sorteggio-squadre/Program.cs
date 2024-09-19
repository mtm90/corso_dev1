using Spectre.Console;

List<string> devclass = new List<string>
{
    "Francesco",
    "Mattia",
    "Ginevra",
    "Christian",
    "Serghej",
    "Matteo",
};

List<string> team1 = new();
List<string> team2 = new();
List<string> team3 = new();
Random random = new();
int teamSelector = 1; // Variable to alternate between teams

while (devclass.Count > 0)
{
    int randomIndex = random.Next(0, devclass.Count);

    if (teamSelector == 1)
    {
        team1.Add(devclass[randomIndex]);
        teamSelector = 2; // Switch to team 2
    }
    else if (teamSelector == 2)
    {
        team2.Add(devclass[randomIndex]);
        teamSelector = 3; // Switch to team 3
    }
    else
    {
        team3.Add(devclass[randomIndex]);
        teamSelector = 1; // Switch back to team 1
    }

    devclass.RemoveAt(randomIndex);
}

// Team 1 Table
var table1 = new Table();
table1.AddColumn("[bold red]team 1[/]");
foreach (string member in team1)
{
    table1.AddRow(member);
}
table1.Width(15);
AnsiConsole.Write(table1);

// Team 2 Table
var table2 = new Table();
table2.AddColumn("[bold blue]team 2[/]");
foreach (string member in team2)
{
    table2.AddRow(member);
}
table2.Width(15);
AnsiConsole.Write(table2);

// Team 3 Table
var table3 = new Table();
table3.AddColumn("[bold green]team 3[/]");
foreach (string member in team3)
{
    table3.AddRow(member);
}
table3.Width(15);
AnsiConsole.Write(table3);
