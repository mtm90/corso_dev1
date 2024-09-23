using System.Data.SQLite;

class Database
{
    private SQLiteConnection _connection;

    public Database()
    {
        // Connect to SQLite database and create 'books' table if it doesn't exist
        _connection = new SQLiteConnection("Data Source=database.db");
        _connection.Open();
        var command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS books (id INTEGER PRIMARY KEY AUTOINCREMENT, title TEXT, author TEXT, yearPublished INTEGER, genre TEXT)", _connection);
        command.ExecuteNonQuery();
    }

    // Method to add a book to the database
    public void AddBook(string title, string author, int yearPublished, string genre)
    {
        var command = new SQLiteCommand("INSERT INTO books (title, author, yearPublished, genre) VALUES (@Title, @Author, @YearPublished, @Genre)", _connection);

        // Add parameters to prevent SQL injection
        command.Parameters.AddWithValue("@Title", title);
        command.Parameters.AddWithValue("@Author", author);
        command.Parameters.AddWithValue("@YearPublished", yearPublished);
        command.Parameters.AddWithValue("@Genre", genre);

        command.ExecuteNonQuery(); // Execute the SQL command to insert the book
    }

    public List<Book> GetBooks()
    {
        var command = new SQLiteCommand("SELECT id, title, author, yearPublished, genre FROM books", _connection);
        var reader = command.ExecuteReader();
        var books = new List<Book>();
        while (reader.Read())
        {
            var id = reader.GetInt32(0); // Retrieve the id
            var title = reader.GetString(1); // Retrieve the name
            var author = reader.GetString(2);
            var yearPublished = reader.GetInt32(3);
            var genre = reader.GetString(4);
            books.Add(new Book(id, title, author, yearPublished, genre)); // Create a new User object
        }
        return books;
    }

    public void DeleteBook(int id)
    {
        var command = new SQLiteCommand("DELETE FROM books WHERE id = @Id", _connection);
        command.Parameters.AddWithValue("@Id", id);
        command.ExecuteNonQuery(); // Execute the SQL command to delete the book
    }

    public void UpdateBook(string oldTitle, string newTitle)
    {
        var command = new SQLiteCommand("UPDATE books SET title = @newTitle WHERE title = @oldTitle ", _connection);
        command.Parameters.AddWithValue("@oldTitle", oldTitle);
        command.Parameters.AddWithValue("@newTitle", newTitle);
        command.ExecuteNonQuery();
    }

    public void CloseConnection()
    {
        if (_connection.State != System.Data.ConnectionState.Closed)
        {
            _connection.Close();
        }
    }

    public Book SearchBookByTitle(string title)
    {
        var command = new SQLiteCommand("SELECT * FROM books WHERE title = @title", _connection);
        command.Parameters.AddWithValue("@title", title);
        var reader = command.ExecuteReader();

        if (reader.Read())
        {
            var id = reader.GetInt32(0);
            var foundTitle = reader.GetString(1);
            var author = reader.GetString(2);
            var yearPublished = reader.GetInt32(3);
            var genre = reader.GetString(4);
            return new Book (id, foundTitle, author, yearPublished, genre);
        }
        else
        {
            return null;
        }
    }

}
