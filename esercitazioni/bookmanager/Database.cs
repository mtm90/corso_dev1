using System.Data.SQLite;

class Database
{
    private SQLiteConnection _connection;
    public Database()
    {
        _connection = new SQLiteConnection("Data Source=database.db");
        _connection.Open();
        var command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS books (id INTEGER PRIMARY KEY AUTOINCREMENT, title TEXT, author TEXT, year INT, genre TEXT)", _connection);
        command.ExecuteNonQuery();
    }

    public void AddBook(string title, string author, int yearPublished, string genre)
{
    var command = new SQLiteCommand("INSERT INTO books (title, author, yearPublished, genre) VALUES (@Title, @Author, @YearPublished, @Genre)", _connection);

    // Use parameters to avoid SQL injection
    command.Parameters.AddWithValue("@Title", title);
    command.Parameters.AddWithValue("@Author", author);
    command.Parameters.AddWithValue("@YearPublished", yearPublished);
    command.Parameters.AddWithValue("@Genre", genre);

    command.ExecuteNonQuery(); // Execute the SQL command
}


}