using Spectre.Console;

Random random = new Random();
int myScore = 100;
int computerScore = 100;

AnsiConsole.Write(new FigletText("Dice Game").Centered().Color(Color.Green));
AnsiConsole.MarkupLine("[bold]Welcome to the dice game, you'll be playing against me![/]");

AnsiConsole.MarkupLine("[bold]Here are the scores:[/]");
var barChart = new BarChart()
    .Width(60)
    .Label("[bold]Scores[/]")
    .CenterLabel();
barChart.AddItem("You", myScore, Color.Green);
barChart.AddItem("Me", computerScore, Color.Red);
AnsiConsole.Write(barChart);

while (myScore > 0 && computerScore > 0)
{
    AnsiConsole.MarkupLine("[bold]Press a button to throw the dices[/]");
    Console.ReadKey(true);
    AnsiConsole.WriteLine("");

    int dice1 = random.Next(1, 7);
    int dice2 = random.Next(1, 7);
    int dice3 = random.Next(1, 7);
    int dice4 = random.Next(1, 7);

    var userPanel = new Panel($"[bold]Your dice:[/]\n[green]{dice1}[/], [green]{dice2}[/]");
    userPanel.Header = new PanelHeader("[bold]Your Roll[/]", Justify.Center);
    userPanel.Border = BoxBorder.Double;
    userPanel.Width = 15;
    AnsiConsole.Write(userPanel);

    var computerPanel = new Panel($"[bold]My dice:[/]\n[red]{dice3}[/], [red]{dice4}[/]");
    computerPanel.Header = new PanelHeader("[bold]My Roll[/]", Justify.Center);
    computerPanel.Border = BoxBorder.Double;
    computerPanel.Width = 15;
    AnsiConsole.Write(computerPanel);

    int myPoints = dice1 + dice2;
    int computerPoints = dice3 + dice4;
    int pointsDifference = Math.Abs(myPoints - computerPoints);

    if (myPoints > computerPoints)
    {
        computerScore -= pointsDifference;
    }
    else if (myPoints < computerPoints)
    {
        myScore -= pointsDifference;
    }

    AnsiConsole.MarkupLine("[bold]Here are the new scores:[/]");
    barChart = new BarChart()
        .Width(60)
        .Label("[bold]Scores[/]")
        .CenterLabel();
    barChart.AddItem("You", myScore, Color.Green);
    barChart.AddItem("Me", computerScore, Color.Red);
    AnsiConsole.Write(barChart);
}

if (myScore <= 0)
{
    AnsiConsole.MarkupLine("[red bold]Sorry, I win![/]");
}
else if (computerScore <= 0)
{
    AnsiConsole.MarkupLine("[green bold]Congratulations!! You Win![/]");
}
