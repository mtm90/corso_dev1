class Book 
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int YearPublished { get; set; }
    public string Genre { get; set; }
    public int LibraryId { get; set; } 

    public Book(int id, string title, string author, int yearPublished, string genre, int libraryId)
    {
        Id = id;
        Title = title;
        Author = author;
        YearPublished = yearPublished;
        Genre = genre;
        LibraryId = libraryId;
    }
}