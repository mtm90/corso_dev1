// Data/DatabaseContext.cs
using System.Data.SQLite;

public class DatabaseContext
{
    private readonly string _connectionString;

    public DatabaseContext(string dbPath)
    {
        _connectionString = $"Data Source={dbPath};Version=3;";
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Open();

        string userTable = @"CREATE TABLE IF NOT EXISTS Users (
                                UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                                Username TEXT NOT NULL,
                                Email TEXT NOT NULL,
                                Password TEXT NOT NULL
                            );";

        string movieTable = @"CREATE TABLE IF NOT EXISTS Movies (
                                MovieId INTEGER PRIMARY KEY AUTOINCREMENT,
                                Title TEXT NOT NULL,
                                Genre TEXT NOT NULL,
                                Duration INTEGER NOT NULL
                            );";

        string bookingTable = @"CREATE TABLE IF NOT EXISTS Bookings (
                                BookingId INTEGER PRIMARY KEY AUTOINCREMENT,
                                UserId INTEGER NOT NULL,
                                MovieId INTEGER NOT NULL,
                                BookingDate DATETIME NOT NULL,
                                FOREIGN KEY(UserId) REFERENCES Users(UserId),
                                FOREIGN KEY(MovieId) REFERENCES Movies(MovieId)
                            );";

        using var command = new SQLiteCommand(userTable, connection);
        command.ExecuteNonQuery();

        command.CommandText = movieTable;
        command.ExecuteNonQuery();

        command.CommandText = bookingTable;
        command.ExecuteNonQuery();
    }

    public SQLiteConnection GetConnection()
    {
        return new SQLiteConnection(_connectionString);
    }
}
