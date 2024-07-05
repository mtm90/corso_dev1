using Spectre.Console;


AnsiConsole.Write(new Markup("[27]Hello[/] [bold red]World![/]"));
Console.Clear();


var table = new Table();
table.AddColumn("Nome corso");
table.AddColumn("Anno");
table.AddRow("Corso di informatica", "2024");
AnsiConsole.Write(table);

var nome = AnsiConsole.Prompt(new TextPrompt<string>("Inserisci il tuo nome"));

var continua = AnsiConsole.Confirm("Vuoi continuare?");

var rule = new Rule("[red]Hello[/]");

rule.Justification = Justify.Left;

AnsiConsole.Write(rule);

var panel = new Panel("Questo è un testo all'interno di un pannello");
panel.Border = BoxBorder.Rounded;
AnsiConsole.Write(panel);