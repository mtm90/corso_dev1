using System.Data.SQLite;

// Manages the SQLite database connection and initializes the database tables if they don't exist
public class DatabaseContext
{
    private readonly string _connectionString;

    // Constructor that accepts the path to the SQLite database
    public DatabaseContext(string dbPath)
    {
        _connectionString = $"Data Source={dbPath};Version=3;";  // Set up the connection string
        InitializeDatabase();  // Ensures the database and tables are set up
    }

    // Initializes database by creating tables for users, movies, and bookings
    private void InitializeDatabase()
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        // Updated movie table schema to include the IsBooked column
        string movieTable = @"CREATE TABLE IF NOT EXISTS Movies (
                            MovieId INTEGER PRIMARY KEY AUTOINCREMENT,
                            Title TEXT NOT NULL,
                            Genre TEXT NOT NULL,
                            Duration INTEGER NOT NULL,
                            IsBooked BOOLEAN NOT NULL DEFAULT 0  -- New column for booking status
                        );";

        string userTable = @"CREATE TABLE IF NOT EXISTS Users (
                            UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                            Username TEXT NOT NULL,
                            Email TEXT NOT NULL,
                            Password TEXT NOT NULL
                        );";

        string bookingTable = @"CREATE TABLE IF NOT EXISTS Bookings (
                            BookingId INTEGER PRIMARY KEY AUTOINCREMENT,
                            UserId INTEGER NOT NULL,
                            MovieId INTEGER NOT NULL,
                            BookingDate DATETIME NOT NULL,
                            FOREIGN KEY(UserId) REFERENCES Users(UserId),
                            FOREIGN KEY(MovieId) REFERENCES Movies(MovieId)
                        );";

        // Execute table creation commands
        using var command = new SQLiteCommand(userTable, connection);
        command.ExecuteNonQuery();

        command.CommandText = movieTable;
        command.ExecuteNonQuery();

        command.CommandText = bookingTable;
        command.ExecuteNonQuery();
    }


    // Returns an active SQLite connection to be used by controllers
    public SQLiteConnection GetConnection()
    {
        return new SQLiteConnection(_connectionString);
    }

    // Deletes a booking by its ID
    public void DeleteBooking(int bookingId)
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        string delete = @"DELETE FROM Bookings WHERE BookingId = @BookingId;";
        using var command = new SQLiteCommand(delete, connection);
        command.Parameters.AddWithValue("@BookingId", bookingId);
        command.ExecuteNonQuery();
    }

}
