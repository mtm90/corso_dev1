using System.Data.SQLite; // Namespace for SQLite database functionality

// Controller class responsible for managing movie-related operations
public class MovieController
{
    private readonly DatabaseContext _dbContext; // Instance of DatabaseContext for database interactions
    private readonly MovieView _view;             // Instance of MovieView for user interface interactions

    // Constructor to initialize MovieController with the database context and view
    public MovieController(DatabaseContext dbContext, MovieView view)
    {
        _dbContext = dbContext; // Assign the provided DatabaseContext to the private field
        _view = view;           // Assign the provided MovieView to the private field
    }

    // Adds a new movie to the database.
    // Retrieves movie details from user input through the view and executes an insert query.
    // Displays a success message upon successful addition.    
    public void AddMovie()
    {
        // Get movie details from user input through the view
        Movie newMovie = _view.GetMovieDetailsFromUser();

        using var connection = _dbContext.GetConnection(); // Get a connection to the database
        connection.Open(); // Open the database connection

        // SQL query to insert a new movie into the Movies table
        string query = "INSERT INTO Movies (Title, Genre, Duration) VALUES (@Title, @Genre, @Duration)";
        using var command = new SQLiteCommand(query, connection); // Prepare the SQL command
        // Bind parameters to prevent SQL injection
        command.Parameters.AddWithValue("@Title", newMovie.Title);
        command.Parameters.AddWithValue("@Genre", newMovie.Genre);
        command.Parameters.AddWithValue("@Duration", newMovie.Duration);
        command.ExecuteNonQuery(); // Execute the insert command

        _view.ShowMovieAddedSuccess(newMovie); // Display a success message to the user through the view
    }

    // Lists all movies from the database.
    // Retrieves all movie records and displays them through the view.
    public void ListAllMovies()
    {
        var movies = new List<Movie>(); // Initialize a list to store movies

        using var connection = _dbContext.GetConnection(); // Get a connection to the database
        connection.Open(); // Open the database connection

        // SQL query to select all movies from the Movies table, including the IsBooked column
        string query = "SELECT MovieId, Title, Genre, Duration, IsBooked FROM Movies";
        using var command = new SQLiteCommand(query, connection); // Prepare the SQL command
        using var reader = command.ExecuteReader(); // Execute the command and get a data reader

        // Read each movie record and add it to the movies list
        while (reader.Read())
        {
            movies.Add(new Movie
            {
                MovieId = Convert.ToInt32(reader["MovieId"]), // Convert the movie ID to int and store
                Title = reader["Title"].ToString(),           // Store the movie title
                Genre = reader["Genre"].ToString(),           // Store the movie genre
                Duration = Convert.ToInt32(reader["Duration"]), // Convert the duration to int and store
                IsBooked = Convert.ToBoolean(reader["IsBooked"]) // Read and store the IsBooked status
            });
        }

        _view.DisplayMovies(movies); // Call the view method to show the movies
    }

    // Initiates the search for movies based on user input.
    // Prompts the user for search criteria and calls the appropriate search method.
    public void SearchMovies()
    {
        Console.WriteLine("Search Movies");
        Console.WriteLine("1. Search by Title");
        Console.WriteLine("2. Search by Genre");
        Console.WriteLine("Choose an option (1 or 2):");

        string option = Console.ReadLine(); // Read user input for search option

        // Execute the appropriate search method based on user input
        switch (option)
        {
            case "1":
                SearchByTitle(); // Call the method to search by title
                break;
            case "2":
                SearchByGenre(); // Call the method to search by genre
                break;
            default:
                Console.WriteLine("Invalid option. Please try again."); // Inform user of invalid input
                break;
        }
    }

    // Searches for movies by title and displays the results.
    // Prompts the user for a title and retrieves matching movies from the database.
    public void SearchByTitle()
    {
        Console.Write("Enter movie title (or part of it): "); // Prompt user for movie title
        string title = Console.ReadLine(); // Read user input for title

        var movies = new List<Movie>(); // Initialize a list to store search results

        using var connection = _dbContext.GetConnection(); // Get a connection to the database
        connection.Open(); // Open the database connection

        // SQL query to select movies matching the title from the Movies table
        string query = "SELECT * FROM Movies WHERE Title LIKE @Title";
        using var command = new SQLiteCommand(query, connection); // Prepare the SQL command
        command.Parameters.AddWithValue("@Title", "%" + title + "%"); // Bind the title parameter with wildcard

        using var reader = command.ExecuteReader(); // Execute the command and get a data reader

        // Read each movie record that matches the title and add it to the list
        while (reader.Read())
        {
            movies.Add(new Movie
            {
                MovieId = Convert.ToInt32(reader["MovieId"]), // Convert the movie ID to int and store
                Title = reader["Title"].ToString(),           // Store the movie title
                Genre = reader["Genre"].ToString(),           // Store the movie genre
                Duration = Convert.ToInt32(reader["Duration"]), // Convert the duration to int and store
                IsBooked = Convert.ToBoolean(reader["IsBooked"]) // Read and store the IsBooked status
            });
        }

        _view.DisplaySearchResults(movies); // Call the view method to display search results
    }

    // Searches for movies by genre and displays the results.
    // Prompts the user for a genre and retrieves matching movies from the database.

    public void SearchByGenre()
    {
        Console.Write("Enter movie genre: "); // Prompt user for movie genre
        string genre = Console.ReadLine(); // Read user input for genre

        var movies = new List<Movie>(); // Initialize a list to store search results

        using var connection = _dbContext.GetConnection(); // Get a connection to the database
        connection.Open(); // Open the database connection

        // SQL query to select movies matching the genre from the Movies table
        string query = "SELECT * FROM Movies WHERE Genre LIKE @Genre";
        using var command = new SQLiteCommand(query, connection); // Prepare the SQL command
        command.Parameters.AddWithValue("@Genre", "%" + genre + "%"); // Bind the genre parameter with wildcard

        using var reader = command.ExecuteReader(); // Execute the command and get a data reader

        // Read each movie record that matches the genre and add it to the list
        while (reader.Read())
        {
            movies.Add(new Movie
            {
                MovieId = Convert.ToInt32(reader["MovieId"]), // Convert the movie ID to int and store
                Title = reader["Title"].ToString(),           // Store the movie title
                Genre = reader["Genre"].ToString(),           // Store the movie genre
                Duration = Convert.ToInt32(reader["Duration"]) // Convert the duration to int and store
            });
        }

        _view.DisplaySearchResults(movies); // Call the view method to display search results
    }

    // Orders movies by duration in descending order and displays the results.
    // Retrieves all movies sorted by duration and displays them through the view.
    public void OrderMoviesByDuration()
    {
        var movies = new List<Movie>(); // Initialize a list to store movies

        using var connection = _dbContext.GetConnection(); // Get a connection to the database
        connection.Open(); // Open the database connection

        // SQL query to select all movies ordered by duration in descending order
        string query = "SELECT * FROM Movies ORDER BY Duration DESC";
        using var command = new SQLiteCommand(query, connection); // Prepare the SQL command

        using var reader = command.ExecuteReader(); // Execute the command and get a data reader

        // Read each movie record and add it to the list
        while (reader.Read())
        {
            movies.Add(new Movie
            {
                MovieId = Convert.ToInt32(reader["MovieId"]), // Convert the movie ID to int and store
                Title = reader["Title"].ToString(),           // Store the movie title
                Genre = reader["Genre"].ToString(),           // Store the movie genre
                Duration = Convert.ToInt32(reader["Duration"]), // Convert the duration to int and store
                IsBooked = Convert.ToBoolean(reader["IsBooked"]) // Read and store the IsBooked status
            });
        }

        _view.DisplaySearchResults(movies); // Call the view method to display ordered movie results
    }
}

