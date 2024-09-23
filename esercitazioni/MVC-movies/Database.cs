using System.Data.SQLite;
class Database
{
    private SQLiteConnection _connection;

    public Database()
    {
        _connection = new SQLiteConnection("Data Source=database.db");
        _connection.Open();

        var command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS movies (id INTEGER PRIMARY KEY AUTOINCREMENT, title TEXT, director TEXT, genre TEXT, year INTEGER, rating INTEGER, actors TEXT)", _connection);
        command.ExecuteNonQuery();
    }

    public void AddMovie(string title, string director, string genre, int year, int rating, List<string> actors)
    {
        var actorsString = string.Join(",", actors); // Converts List<string> to a single comma-separated string

        var command = new SQLiteCommand("INSERT INTO movies (title, director, genre, year, rating, actors) VALUES (@title, @director, @genre, @year, @rating, @actors)",_connection);
        command.Parameters.AddWithValue("@title", title);
        command.Parameters.AddWithValue("@director", director);
        command.Parameters.AddWithValue("@genre", genre);
        command.Parameters.AddWithValue("@year", year);
        command.Parameters.AddWithValue("@rating", rating);
        command.Parameters.AddWithValue("@actors", actorsString);
        command.ExecuteNonQuery();
    }
    public List<Movie> GetMovies()
    {
        var command = new SQLiteCommand("SELECT * FROM movies", _connection);
        var reader = command.ExecuteReader();
        var movies = new List<Movie>();
        while (reader.Read())
        {
            var id = reader.GetInt32(0);
            var title = reader.GetString(1);
            var director = reader.GetString(2);
            var genre = reader.GetString(3);
            var year = reader.GetInt32(4);
            var rating = reader.GetInt32(5);
            var actors = reader.GetString(6);
            var actorsList = actors.Split(',').ToList(); // Converts comma-separated string back to List<string>

            movies.Add(new Movie(id, title, director, genre, year, rating, actorsList));


        }
        return movies;
    }
}