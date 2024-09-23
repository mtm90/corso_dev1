class Controller
{
    private Database _db;
    private View _view;

    public Controller(Database db, View view)
    {
        _db = db;
        _view = view;
    }

    public void MainMenu()
    {
        while (true)
        {
            _view.ShowMainMenu();
            var choice = _view.GetInput("Choose an option:");

            if (choice == "1")
            {
                AddBook(); // Call AddBook method
            }
            else if (choice == "2")
            {
                ShowBook();
            }
            else if (choice == "3")
            {
                DeleteBook();
            }
            else if (choice == "4")
            {
                UpdateBook();
            }
            else if (choice == "5")
            {
                SearchBookByTitle();
            }
            else if (choice == "6")
            {
                _db.CloseConnection();
                break; // Exit the program
            }
        }
    }


public static string SanitizeInput(string input)
{
    // Simple sanitization (you can use more advanced libraries for this)
    return input.Replace("<", "").Replace(">", "").Trim();
}
    // Add a book by getting user input and passing it to the database
    private void AddBook()
{
    string title = "";
    string author = "";
    int year = -1;
    string genre = "";

    // Loop until valid title is entered
    while (string.IsNullOrWhiteSpace(title))
    {
        title = SanitizeInput(_view.GetInput("Enter the book title:"));
        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Please insert a valid title.");
        }
    }

    // Loop until valid author is entered
    while (string.IsNullOrWhiteSpace(author))
    {
        author = SanitizeInput(_view.GetInput("Enter the author:"));
        if (string.IsNullOrWhiteSpace(author))
        {
            Console.WriteLine("Please insert a valid author.");
        }
    }

    // Loop until valid year is entered
    while (year < 0)
    {
        string yearInput = SanitizeInput(_view.GetInput("Enter the year of publication:"));
        if (!int.TryParse(yearInput, out year) || year < 0)
        {
            Console.WriteLine("Please insert a valid year.");
            year = -1; // Reset year if invalid
        }
    }

    // Loop until valid genre is entered
    while (string.IsNullOrWhiteSpace(genre))
    {
        genre = SanitizeInput(_view.GetInput("Enter the genre:"));
        if (string.IsNullOrWhiteSpace(genre))
        {
            Console.WriteLine("Please insert a valid genre.");
        }
    }

    // Call the database method to add the book
    _db.AddBook(title, author, year, genre);
    Console.WriteLine("Book added successfully!");
}



    private void ShowBook()
    {
        var books = _db.GetBooks();
        _view.ShowBooks(books);
    }

    private void DeleteBook()
    {
        Console.WriteLine("Enter the ID of the book to delete:");
        int id;
        while (!int.TryParse(_view.GetInput("ID:"), out id))
        {
            Console.WriteLine("Invalid ID. Please enter a valid number.");
        }

        _db.DeleteBook(id); // Call the database method to delete the book
        Console.WriteLine("Book deleted successfully!");
    }

    private void UpdateBook()
    {
        string oldTitle = _view.GetInput("Enter the title of the book u want to update");
        string newTitle = _view.GetInput("Enter the new title of the book");
        _db.UpdateBook(oldTitle, newTitle);

    }

    private void SearchBookByTitle()
    {
        string title = _view.GetInput("Enter the title of the book u want to search");
        var book = _db.SearchBookByTitle(title);
        if (book != null)
        {
            _view.ShowBooks(new List<Book> { book });
        }
        else
        {
            Console.WriteLine("Book not found");
        }
    }

}
