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

            switch (choice)
            {
                case "1":
                    AddLibrary();
                    break;
                case "2":
                    AddBookToLibrary();
                    break;
                case "3":
                    ViewBooksInLibrary();
                    break;
                case "4":
                    _db.CloseConnection();
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private void AddLibrary()
    {
        var name = _view.GetInput("Enter the library name:");
        _db.AddLibrary(name);
        Console.WriteLine("Library added successfully!");
    }

    private void AddBookToLibrary()
    {
        var libraries = _db.GetLibraries();
        if (libraries.Count == 0)
        {
            Console.WriteLine("No libraries available. Please add a library first.");
            return;
        }

        _view.ShowLibraries(libraries);
        var libraryId = int.Parse(_view.GetInput("Enter the library ID to add the book to:"));

        var title = _view.GetInput("Enter the book title:");
        var author = _view.GetInput("Enter the author:");
        var year = int.Parse(_view.GetInput("Enter the year of publication:"));
        var genre = _view.GetInput("Enter the genre:");

        _db.AddBook(title, author, year, genre, libraryId);
        Console.WriteLine("Book added successfully to the library!");
    }

    private void ViewBooksInLibrary()
    {
        var libraries = _db.GetLibraries();
        if (libraries.Count == 0)
        {
            Console.WriteLine("No libraries available.");
            return;
        }

        _view.ShowLibraries(libraries);
        var libraryId = int.Parse(_view.GetInput("Enter the library ID to view books:"));

        var books = _db.GetBooksByLibrary(libraryId);
        _view.ShowBooks(books);
    }
}