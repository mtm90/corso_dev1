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
        table1.Width(15);
        AnsiConsole.Write(table1);

        var table2 = new Table();
        table2.AddColumn("[bold blue]team 2[/]");

        foreach (string member in team2)
        {
            table2.AddRow(member);

        }
        table2.Width(15);
        AnsiConsole.Write(table2);


/*
var table5 = new Table();
table5.AddColumn("Nome");
table5.AddColumn("Cognome");

var partecipanti = new Dictionary<string, string> {
    {"Mario", "Rossi"},
    {"Luca", "Verdi"},
    {"Andrea", "Bianchi"}
};

foreach (var member in partecipanti)
{
    table5.AddRow(member.Key, member.Value);
}
AnsiConsole.Write(table5);

var table6 = new Table();  
table6.AddColumn("Nome");
table6.AddColumn("Cognome");
table6.AddColumn("Anno di nascita");

var partecipanti2 = new Dictionary<string, (string, int)>
{
    {"Mario", ("Rossi", 1990)},
    {"Luca", ("Bianchi", 1980)},
    {"Paolo", ("Verdi", 1970)}
};

foreach(var member in partecipanti2)
{
    table6.AddRow(member.Key, member.Value.Item1, member.Value.Item2.ToString());
}

AnsiConsole.Write(table6);

var table7 = new Table();
table7.AddColumn("Nome");
table7.AddColumn("Soprannome");
table7.AddColumn("Cognome");
table7.AddColumn("Anno di nascita");


var partecipanti3 = new Dictionary<(string, string), (string, int)>
{
    {("Mario","Ciccio"), ("Rossi", 1990)},
    {("Luca", "gigetto"), ("Bianchi", 1980)},
    {("Paolo", "nan"), ("Verdi", 1970)}
};

foreach (var member in partecipanti3)
{
    table7.AddRow(member.Key.Item1, member.Key.Item2, member.Value.Item1, member.Value.Item2.ToString());
}

AnsiConsole.Write(table7);
*/