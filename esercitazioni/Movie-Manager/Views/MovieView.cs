using Spectre.Console; // Namespace for creating enhanced console applications

// View class responsible for managing user interactions related to movies
public class MovieView
{
    // Collects movie details (title, genre, and duration) from the user
    public Movie GetMovieDetailsFromUser()
    {
        // Prompt the user to enter the movie title.
        // This will be the name of the movie and is required for identification.
        string title = AnsiConsole.Ask<string>("Enter [green]movie title[/]:"); // Get movie title
        
        // Prompt the user to enter the movie genre.
        // This allows categorizing the movie and can aid in filtering/searching later.
        string genre = AnsiConsole.Ask<string>("Enter [green]movie genre[/]:"); // Get movie genre
        
        // Prompt the user to enter the movie duration in minutes.
        // Duration is essential for scheduling and can help users decide on watching it.
        int duration = AnsiConsole.Ask<int>("Enter [green]movie duration[/] (in minutes):"); // Get movie duration in minutes

        // Return a new Movie object populated with the collected details.
        // This encapsulates the data in an object-oriented manner for better management.
        return new Movie
        {
            Title = title,       // Assign the entered title to the Movie object
            Genre = genre,       // Assign the entered genre to the Movie object
            Duration = duration   // Assign the entered duration to the Movie object
        };
    }

    // Displays a success message for a newly added movie
    public void ShowMovieAddedSuccess(Movie movie)
    {
        Console.Clear(); // Clear the console for a fresh display of the success message
        
        // Display a success message indicating that the movie has been added successfully.
        // The message includes the title of the added movie and is styled with color for emphasis.
        AnsiConsole.Markup($"[green]Movie '{movie.Title}'[/] added successfully!");
        
        // Add extra line breaks for visual spacing in the console output.
        // This enhances the user experience by separating messages for clarity.
        Console.WriteLine(""); // Add spacing
        Console.WriteLine(""); // Add spacing
        
        // Prompt the user to continue by pressing Enter.
        // This gives the user a chance to acknowledge the message before moving on.
        AnsiConsole.Markup($"Press [green]Enter[/] to continue"); // Prompt to continue
        
        // Wait for the user to press Enter before proceeding.
        // This prevents the console from immediately moving on, allowing the user to read the message.
        Console.ReadLine(); // Wait for user input
    }

    // Displays all movies in a table format
    public void DisplayMovies(List<Movie> movies)
    {
        // Create a new table instance to visually display the movies.
        // This presents the data in an organized format, making it easier to read.
        var table = new Table(); // Create a new table instance
        
        // Define the columns for the table to represent various attributes of each movie.
        table.AddColumn("Movie ID");           // Add column for Movie ID
        table.AddColumn("Title");               // Add column for Movie Title
        table.AddColumn("Genre");               // Add column for Movie Genre
        table.AddColumn("Duration (mins)");     // Add column for Movie Duration
        table.AddColumn("Is Booked");           // Add column for Booking Status

        // Iterate over each movie in the provided list of movies.
        // This allows for populating the table with all movie records.
        foreach (var movie in movies)
        {
            // Add a new row to the table for each movie, containing relevant details.
            // Each movie's information is converted to a string for display purposes.
            table.AddRow(
                movie.MovieId.ToString(), // Convert Movie ID to string for display
                movie.Title,              // Display Movie Title
                movie.Genre,              // Display Movie Genre
                movie.Duration.ToString(), // Convert Duration to string for display
                movie.IsBooked ? "[red]Yes[/]" : "[green]No[/]" // Display booking status with color coding
            );
        }

        // Finally, display the constructed table in the console.
        // This renders the table with all movie information for the user to view.
        AnsiConsole.Write(table); // Display the constructed table in the console
    }

    // Displays search results for movies in a table format
    public void DisplaySearchResults(List<Movie> movies)
    {
        // Check if no movies were found in the search results.
        // This helps to provide feedback to the user if their search yielded no results.
        if (movies.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No movies found.[/]"); // Inform user if no results were found
            return; // Exit the method early, as there is nothing to display
        }

        // Create a new table instance for displaying the search results.
        var table = new Table(); // Create a new table instance
        table.AddColumn("Movie ID");           // Add column for Movie ID
        table.AddColumn("Title");               // Add column for Movie Title
        table.AddColumn("Genre");               // Add column for Movie Genre
        table.AddColumn("Duration (mins)");     // Add column for Movie Duration
        table.AddColumn("Is Booked");           // Add column for Booking Status

        // Iterate over each movie in the search results.
        // This populates the table with the relevant movie data.
        foreach (var movie in movies)
        {
            table.AddRow(
                movie.MovieId.ToString(), // Convert Movie ID to string for display
                movie.Title,              // Display Movie Title
                movie.Genre,              // Display Movie Genre
                movie.Duration.ToString(), // Convert Duration to string for display
                movie.IsBooked ? "[red]Yes[/]" : "[green]No[/]" // Display booking status with color coding
            );
        }

        // Display the constructed table with the search results in the console.
        // This allows the user to see the results of their search in an organized format.
        AnsiConsole.Write(table); // Display the constructed table in the console
    }
}
