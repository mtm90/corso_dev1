using Spectre.Console;

AnsiConsole.Clear();

// esempio di testo semplice
AnsiConsole.WriteLine("Testo con nuova linea");
AnsiConsole.Write("Testo senza nuova linea");

AnsiConsole.WriteLine();

// esempio di testo formattato
AnsiConsole.Markup("[underline red]Testo sottolineato e rosso[/]\n");
AnsiConsole.Markup("[slateblue3_1]Hello[/] [28]World![/]\n");

AnsiConsole.WriteLine();

var continua = AnsiConsole.Confirm("Vuoi continuare?");

// esempio di tabella
var table = new Table();
table.AddColumn("Nome corso");
table.AddColumn("Anno");
table.AddRow("Corso di informatica", "2024");
AnsiConsole.Write(table);

// var rule = new Rule();
var rule = new Rule("[red]Hello[/]");
rule.Justification = Justify.Left; // Justify.Center, Justify.Right
AnsiConsole.Write(rule);

// esempio di prompt
var nome = AnsiConsole.Prompt(new TextPrompt<string>("Inserisci il tuo nome:"));

// esempio di panel
var panel = new Panel("Questo è un testo all'interno di un pannello");
panel.Border = BoxBorder.Heavy;
AnsiConsole.Write(panel);

// esempio di progress bar
AnsiConsole.Write(new BarChart()
    .Width(60)
    .Label("[green bold underline]Number of fruits[/]")
    .CenterLabel()
    .AddItem("Apple", 12, Color.Yellow)
    .AddItem("Orange", 54, Color.Green)
    .AddItem("Banana", 33, Color.Red));

// esempio di calendario
var calendar = new Calendar(DateTime.Now.Year, DateTime.Now.Month);
// localizzazioni
calendar.Culture("it-IT");
// impostazioni
calendar.AddCalendarEvent(2024, 7, 11);
calendar.HighlightStyle(Style.Parse("blue bold"));
AnsiConsole.Write(calendar);

// esempio di layout 

// Create the layout
var layout = new Layout("Root")
    .SplitColumns(
        new Layout("Left"),
        new Layout("Right")
            .SplitRows(
                new Layout("Top"),
                new Layout("Bottom")));

// Update the left column
layout["Left"].Update(
    new Panel(
        Align.Center(
            new Markup("Hello [blue]World![/]"),
            VerticalAlignment.Middle))
        .Expand());

// Render the layout
AnsiConsole.Write(layout);


AnsiConsole.Write(
    new FigletText("Hello")
        .LeftJustified()
        .Color(Color.Red));

        AnsiConsole.MarkupLine("Hello :warning:");