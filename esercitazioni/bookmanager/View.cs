class View
{
    public void ShowMainMenu()
    {
        Console.WriteLine("1. Add Library");
        Console.WriteLine("2. Add Book to Library");
        Console.WriteLine("3. View Books in Library");
        Console.WriteLine("4. Exit");
    }

    public string GetInput(string prompt)
    {
        Console.WriteLine(prompt);
        return Console.ReadLine();
    }

    public void ShowLibraries(List<Library> libraries)
    {
        foreach (var library in libraries)
        {
            Console.WriteLine($"ID: {library.Id}, Name: {library.Name}");
        }
    }

    public void ShowBooks(List<Book> books)
    {
        foreach (var book in books)
        {
            Console.WriteLine($"ID: {book.Id}, Title: {book.Title}, Author: {book.Author}, Year: {book.YearPublished}, Genre: {book.Genre}, Library ID: {book.LibraryId}");
        }
    }
}