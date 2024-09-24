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
}
