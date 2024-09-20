class View
{
    
    public void ShowMainMenu()
    {
        Console.WriteLine("1. Add Book");
        Console.WriteLine("2. View books");
        Console.WriteLine("3. Delete book");
        Console.WriteLine("4. Exit");
    }

    public string GetInput(string prompt)
    {
        Console.WriteLine(prompt);
        return Console.ReadLine();
    }

    public void ShowBooks(List<Book> books)
    {
        foreach (var book in books)
        {
            Console.WriteLine($"ID: {book.Id}, Title: {book.Title}, Author: {book.Author}, Year: {book.YearPublished}, Genre: {book.Genre}");  
        }
    }

    public void ShowDeleteBookMenu()
{
    Console.WriteLine("Enter the ID of the book to delete:");
}

}
