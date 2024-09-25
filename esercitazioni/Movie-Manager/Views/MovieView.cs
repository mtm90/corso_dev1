using Spectre.Console;

public class MovieView
{
    // Collects movie details (title, genre, and duration) from the user
    public Movie GetMovieDetailsFromUser()
    {
        string title = AnsiConsole.Ask<string>("Enter [green]movie title[/]:");
        string genre = AnsiConsole.Ask<string>("Enter [green]movie genre[/]:");
        int duration = AnsiConsole.Ask<int>("Enter [green]movie duration[/] (in minutes):");

        return new Movie
        {
            Title = title,
            Genre = genre,
            Duration = duration
        };
    }

    // Displays a success message for a newly added movie
    public void ShowMovieAddedSuccess(Movie movie)
    {
        AnsiConsole.Markup($"[green]Movie '{movie.Title}'[/] added successfully!");
    }

    // Displays all movies in a table format
    public void DisplayMovies(List<Movie> movies)
{
    var table = new Table();
    table.AddColumn("Movie ID");
    table.AddColumn("Title");
    table.AddColumn("Genre");
    table.AddColumn("Duration (mins)");
    table.AddColumn("Is Booked");  // New column

    // Add each movie's data to the table, including the IsBooked status
    foreach (var movie in movies)
    {
        table.AddRow(
            movie.MovieId.ToString(),
            movie.Title,
            movie.Genre,
            movie.Duration.ToString(),
            movie.IsBooked ? "[red]Yes[/]" : "[green]No[/]"  // Display booking status with color
        );
    }

    AnsiConsole.Write(table);
}


    // Displays search results for movies in a table format
    public void DisplaySearchResults(List<Movie> movies)
{
    if (movies.Count == 0)
    {
        AnsiConsole.MarkupLine("[red]No movies found.[/]");
        return;
    }

    var table = new Table();
    table.AddColumn("Movie ID");
    table.AddColumn("Title");
    table.AddColumn("Genre");
    table.AddColumn("Duration (mins)");
    table.AddColumn("Is Booked");  // New column

    foreach (var movie in movies)
    {
        table.AddRow(
            movie.MovieId.ToString(),
            movie.Title,
            movie.Genre,
            movie.Duration.ToString(),
            movie.IsBooked ? "[red]Yes[/]" : "[green]No[/]"  // Display booking status with color
        );
    }

    AnsiConsole.Write(table);
}

}
