using Spectre.Console; // Namespace for creating enhanced console applications

// View class responsible for managing user interactions related to movies
public class MovieView
{
    // Collects movie details (title, genre, and duration) from the user
    public Movie GetMovieDetailsFromUser()
    {
        // Prompt the user to enter movie details
        string title = AnsiConsole.Ask<string>("Enter [green]movie title[/]:"); // Get movie title
        string genre = AnsiConsole.Ask<string>("Enter [green]movie genre[/]:"); // Get movie genre
        int duration = AnsiConsole.Ask<int>("Enter [green]movie duration[/] (in minutes):"); // Get movie duration in minutes

        // Return a new Movie object with the collected details
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
        Console.Clear(); // Clear the console for a fresh display
        // Display success message with movie title
        AnsiConsole.Markup($"[green]Movie '{movie.Title}'[/] added successfully!");
        Console.WriteLine(""); // Add spacing
        Console.WriteLine(""); // Add spacing
        AnsiConsole.Markup($"Press [green]Enter[/] to continue"); // Prompt to continue
        Console.ReadLine(); // Wait for user input
    }

    // Displays all movies in a table format
    public void DisplayMovies(List<Movie> movies)
    {
        var table = new Table(); // Create a new table instance
        table.AddColumn("Movie ID"); // Add column for Movie ID
        table.AddColumn("Title"); // Add column for Movie Title
        table.AddColumn("Genre"); // Add column for Movie Genre
        table.AddColumn("Duration (mins)"); // Add column for Movie Duration
        table.AddColumn("Is Booked");  // Add column for Booking Status

        // Add each movie's data to the table, including the IsBooked status
        foreach (var movie in movies)
        {
            table.AddRow(
                movie.MovieId.ToString(), // Convert Movie ID to string
                movie.Title,              // Movie Title
                movie.Genre,              // Movie Genre
                movie.Duration.ToString(), // Convert Duration to string
                movie.IsBooked ? "[red]Yes[/]" : "[green]No[/]" // Display booking status with color
            );
        }

        // Display the constructed table in the console
        AnsiConsole.Write(table);
    }

    // Displays search results for movies in a table format
    public void DisplaySearchResults(List<Movie> movies)
    {
        // Check if no movies were found
        if (movies.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No movies found.[/]"); // Inform user if no results
            return; // Exit the method
        }

        var table = new Table(); // Create a new table instance
        table.AddColumn("Movie ID"); // Add column for Movie ID
        table.AddColumn("Title"); // Add column for Movie Title
        table.AddColumn("Genre"); // Add column for Movie Genre
        table.AddColumn("Duration (mins)"); // Add column for Movie Duration
        table.AddColumn("Is Booked");  // Add column for Booking Status

        // Add each movie's data to the table, including the IsBooked status
        foreach (var movie in movies)
        {
            table.AddRow(
                movie.MovieId.ToString(), // Convert Movie ID to string
                movie.Title,              // Movie Title
                movie.Genre,              // Movie Genre
                movie.Duration.ToString(), // Convert Duration to string
                movie.IsBooked ? "[red]Yes[/]" : "[green]No[/]" // Display booking status with color
            );
        }

        // Display the constructed table in the console
        AnsiConsole.Write(table);
    }
}
