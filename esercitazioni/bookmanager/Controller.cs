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
                break; // Exit the program
            }
        }
    }

    // Add a book by getting user input and passing it to the database
    private void AddBook()
    {
        string title = _view.GetInput("Enter the book title:");
        string author = _view.GetInput("Enter the author:");
        int year = int.Parse(_view.GetInput("Enter the year of publication:"));
        string genre = _view.GetInput("Enter the genre:");

        _db.AddBook(title, author, year, genre); // Call the database method to add the book
        Console.WriteLine("Book added successfully!");
    }

    private void ShowBook()
    {
        var books = _db.GetBooks();
        _view.ShowBooks(books);
    }

    private void DeleteBook()
{
    _view.ShowDeleteBookMenu();
    int id;
    while (!int.TryParse(_view.GetInput("ID:"), out id))
    {
        Console.WriteLine("Invalid ID. Please enter a valid number.");
    }

    _db.DeleteBook(id); // Call the database method to delete the book
    Console.WriteLine("Book deleted successfully!");
}

}
