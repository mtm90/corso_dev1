using System.Data.SQLite;
public class MovieController
{
    private readonly DatabaseContext _dbContext;
    private readonly MovieView _view;

    public MovieController(DatabaseContext dbContext, MovieView view)
    {
        _dbContext = dbContext;
        _view = view;
    }

    // Adds a new movie to the database
    public void AddMovie()
    {
        Movie newMovie = _view.GetMovieDetailsFromUser();

        using var connection = _dbContext.GetConnection();
        connection.Open();

        string query = "INSERT INTO Movies (Title, Genre, Duration) VALUES (@Title, @Genre, @Duration)";
        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@Title", newMovie.Title);
        command.Parameters.AddWithValue("@Genre", newMovie.Genre);
        command.Parameters.AddWithValue("@Duration", newMovie.Duration);
        command.ExecuteNonQuery();

        _view.ShowMovieAddedSuccess(newMovie);
    }

    // Lists all movies
    public void ListAllMovies()
    {
        var movies = new List<Movie>();

        using var connection = _dbContext.GetConnection();
        connection.Open();

        string query = "SELECT MovieId, Title, Genre, Duration, IsBooked FROM Movies";  // Fetch IsBooked column
        using var command = new SQLiteCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            movies.Add(new Movie
            {
                MovieId = Convert.ToInt32(reader["MovieId"]),
                Title = reader["Title"].ToString(),
                Genre = reader["Genre"].ToString(),
                Duration = Convert.ToInt32(reader["Duration"]),
                IsBooked = Convert.ToBoolean(reader["IsBooked"])  // Read the IsBooked status
            });
        }

        _view.DisplayMovies(movies);
    }

    // Search movies by title or genre
    public void SearchMovies()
    {
        Console.WriteLine("Search Movies");
        Console.WriteLine("1. Search by Title");
        Console.WriteLine("2. Search by Genre");
        Console.WriteLine("Choose an option (1 or 2):");

        string option = Console.ReadLine();

        switch (option)
        {
            case "1":
                SearchByTitle();
                break;
            case "2":
                SearchByGenre();
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }

    // Search for movies by title and display the results in a table
    public void SearchByTitle()
    {
        Console.Write("Enter movie title (or part of it): ");
        string title = Console.ReadLine();

        var movies = new List<Movie>();

        using var connection = _dbContext.GetConnection();
        connection.Open();

        string query = "SELECT * FROM Movies WHERE Title LIKE @Title";
        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@Title", "%" + title + "%");

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            movies.Add(new Movie
            {
                MovieId = Convert.ToInt32(reader["MovieId"]),
                Title = reader["Title"].ToString(),
                Genre = reader["Genre"].ToString(),
                Duration = Convert.ToInt32(reader["Duration"]),
                IsBooked = Convert.ToBoolean(reader["IsBooked"])  // Read the IsBooked status
            });
        }

        _view.DisplaySearchResults(movies);
    }

    // Search for movies by genre and display the results in a table
    public void SearchByGenre()
    {
        Console.Write("Enter movie genre: ");
        string genre = Console.ReadLine();

        var movies = new List<Movie>();

        using var connection = _dbContext.GetConnection();
        connection.Open();

        string query = "SELECT * FROM Movies WHERE Genre LIKE @Genre";
        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@Genre", "%" + genre + "%");

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            movies.Add(new Movie
            {
                MovieId = Convert.ToInt32(reader["MovieId"]),
                Title = reader["Title"].ToString(),
                Genre = reader["Genre"].ToString(),
                Duration = Convert.ToInt32(reader["Duration"])
            });
        }

        // Pass the search results to the view to display in a table
        _view.DisplaySearchResults(movies);
    }

    public void OrderMoviesByDuration()
    {
        var movies = new List<Movie>();

        using var connection = _dbContext.GetConnection();
        connection.Open();

        string query = "SELECT * FROM Movies ORDER BY Duration DESC";
        using var command = new SQLiteCommand(query, connection);

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            movies.Add(new Movie
            {
                MovieId = Convert.ToInt32(reader["MovieId"]),
                Title = reader["Title"].ToString(),
                Genre = reader["Genre"].ToString(),
                Duration = Convert.ToInt32(reader["Duration"]),
                IsBooked = Convert.ToBoolean(reader["IsBooked"])  // Read the IsBooked status
            });
        }

        _view.DisplaySearchResults(movies);
    }
}
