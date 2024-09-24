public class MovieController
{
    private readonly DatabaseContext _dbContext;
    private readonly MovieView _view;

    public MovieController(DatabaseContext dbContext, MovieView view)
    {
        _dbContext = dbContext;
        _view = view;
    }

    public void AddMovie()
    {
        // Call the view to get movie details from the user
        Movie newMovie = _view.GetMovieDetailsFromUser();

        using var connection = _dbContext.GetConnection();
        connection.Open();

        string query = "INSERT INTO Movies (Title, Genre, Duration) VALUES (@Title, @Genre, @Duration)";
        using var command = new System.Data.SQLite.SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@Title", newMovie.Title);
        command.Parameters.AddWithValue("@Genre", newMovie.Genre);
        command.Parameters.AddWithValue("@Duration", newMovie.Duration);

        command.ExecuteNonQuery();

        // Notify the view that the movie was added successfully
        _view.ShowMovieAddedSuccess(newMovie);
    }

    public void ListAllMovies()
    {
        var movies = new List<Movie>();

        using var connection = _dbContext.GetConnection();
        connection.Open();

        string query = "SELECT MovieId, Title, Genre, Duration FROM Movies";
        using var command = new System.Data.SQLite.SQLiteCommand(query, connection);
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

        // Display all movies using the view
        _view.DisplayMovies(movies);
    }

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

    public void SearchByTitle()
    {
        Console.Write("Enter movie title (or part of it): ");
        string title = Console.ReadLine();

        using var connection = _dbContext.GetConnection();
        connection.Open();

        string query = "SELECT * FROM Movies WHERE Title LIKE @Title";
        using var command = new System.Data.SQLite.SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@Title", "%" + title + "%");

        using var reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            Console.WriteLine("Movies Found:");
            while (reader.Read())
            {
                Console.WriteLine($"ID: {reader["MovieId"]}, Title: {reader["Title"]}, Genre: {reader["Genre"]}");
            }
        }
        else
        {
            Console.WriteLine("No movies found with that title.");
        }
    }

    public void SearchByGenre()
    {
        Console.Write("Enter movie genre: ");
        string genre = Console.ReadLine();

        using var connection = _dbContext.GetConnection();
        connection.Open();

        string query = "SELECT * FROM Movies WHERE Genre LIKE @Genre";
        using var command = new System.Data.SQLite.SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@Genre", "%" + genre + "%");

        using var reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            Console.WriteLine("Movies Found:");
            while (reader.Read())
            {
                Console.WriteLine($"ID: {reader["MovieId"]}, Title: {reader["Title"]}, Genre: {reader["Genre"]}");
            }
        }
        else
        {
            Console.WriteLine("No movies found with that genre.");
        }
    }



}
