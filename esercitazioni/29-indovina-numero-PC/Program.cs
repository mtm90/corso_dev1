using Spectre.Console;


Console.Clear();
Random random = new Random();
        int lowerBound = 1;
        int upperBound = 100;
        int computerGuess;
        int guesses = 0;

        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[bold yellow]Think of a number between 1 and 100, I'm gonna try and guess it.[/]");
        Thread.Sleep(2000);
        bool gameIsRunning = true;

        while (gameIsRunning)
        {
            AnsiConsole.Clear();
            computerGuess = random.Next(lowerBound, upperBound + 1);
            
            // Displaying the computer's guess in a box
            var panel = new Panel($"[bold yellow]Is your number {computerGuess}?[/]")
                .Border(BoxBorder.Double)
                .Header("[bold green]Guess[/]");
            AnsiConsole.Write(panel);
            
            guesses++;

            string answer = AnsiConsole.Ask<string>("[bold]Enter [green]'yes'[/] or [red]'no'[/]:[/]");

            if (answer.ToLower() == "yes")
            {
                AnsiConsole.MarkupLine("[bold green]HAHA! I won![/]");
                AnsiConsole.MarkupLine($"[bold blue]It took me {guesses} guesses[/]");
                gameIsRunning = false;
            }
            else if (answer.ToLower() == "no")
            {
                string answer2 = AnsiConsole.Ask<string>($"[bold]Is it [royalblue1]higher[/] or [steelblue1_1]lower[/] than {computerGuess}?[/]");

                if (answer2.ToLower() == "higher")
                {
                    lowerBound = computerGuess + 1;
                }
                else if (answer2.ToLower() == "lower")
                {
                    upperBound = computerGuess - 1;
                }
            }
        }