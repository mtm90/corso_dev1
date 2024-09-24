using System.Data.SQLite;
class Database
{
    private SQLiteConnection _connection;

    public Database()
    {
        _connection = new SQLiteConnection("Data Source=database.db");
        _connection.Open();

        // Create libraries and books tables
        var command = new SQLiteCommand(
            "CREATE TABLE IF NOT EXISTS libraries (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT);" +
            "CREATE TABLE IF NOT EXISTS books (id INTEGER PRIMARY KEY AUTOINCREMENT, title TEXT, author TEXT, yearPublished INTEGER, genre TEXT, libraryId INTEGER, FOREIGN KEY(libraryId) REFERENCES libraries(id));",
            _connection
        );
        command.ExecuteNonQuery();
    }

    // Method to add a library
    public void AddLibrary(string name)
    {
        var command = new SQLiteCommand("INSERT INTO libraries (name) VALUES (@Name)", _connection);
        command.Parameters.AddWithValue("@Name", name);
        command.ExecuteNonQuery();
    }

    // Method to get all libraries
    public List<Library> GetLibraries()
    {
        var command = new SQLiteCommand("SELECT * FROM libraries", _connection);
        var reader = command.ExecuteReader();
        var libraries = new List<Library>();

        while (reader.Read())
        {
            libraries.Add(new Library(reader.GetInt32(0), reader.GetString(1)));
        }

        return libraries;
    }

    // Method to add a book with a libraryId
    public void AddBook(string title, string author, int yearPublished, string genre, int libraryId)
    {
        var command = new SQLiteCommand("INSERT INTO books (title, author, yearPublished, genre, libraryId) VALUES (@Title, @Author, @YearPublished, @Genre, @LibraryId)", _connection);
        command.Parameters.AddWithValue("@Title", title);
        command.Parameters.AddWithValue("@Author", author);
        command.Parameters.AddWithValue("@YearPublished", yearPublished);
        command.Parameters.AddWithValue("@Genre", genre);
        command.Parameters.AddWithValue("@LibraryId", libraryId);
        command.ExecuteNonQuery();
    }

    // Method to get books by library ID
    public List<Book> GetBooksByLibrary(int libraryId)
    {
        var command = new SQLiteCommand("SELECT * FROM books WHERE libraryId = @LibraryId", _connection);
        command.Parameters.AddWithValue("@LibraryId", libraryId);
        var reader = command.ExecuteReader();
        var books = new List<Book>();

        while (reader.Read())
        {
            books.Add(new Book(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetString(4), reader.GetInt32(5)));
        }

        return books;
    }

    // Other existing methods remain unchanged...

    public void CloseConnection()
    {
        if (_connection.State != System.Data.ConnectionState.Closed)
        {
            _connection.Close();
        }
    }
}