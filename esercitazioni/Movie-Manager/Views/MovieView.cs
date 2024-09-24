using Spectre.Console;

public class MovieView
{
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

    public void ShowMovieAddedSuccess(Movie movie)
    {
        AnsiConsole.Markup($"[green]Movie '{movie.Title}'[/] added successfully!");
    }

    public void DisplayMovies(List<Movie> movies)
    {
        var table = new Table();
        table.AddColumn("Movie ID");
        table.AddColumn("Title");
        table.AddColumn("Genre");
        table.AddColumn("Duration (mins)");

        foreach (var movie in movies)
        {
            table.AddRow(
                movie.MovieId.ToString(),
                movie.Title,
                movie.Genre,
                movie.Duration.ToString()
            );
        }

        AnsiConsole.Write(table);
    }
}
