class View
{
    private Database _db;

    public View(Database db)
    {
        _db = db;
    }

    // Show main menu as before
    public void ShowMainMenu()
    {
        Console.WriteLine("1. Add movie");
        Console.WriteLine("2. View movies");
        Console.WriteLine("3. Delete movie");
        Console.WriteLine("4. Exit");
    }

    public string GetInput()
    {
        return Console.ReadLine();
    }

    // Show movies as before
    public void ShowMovies(List<Movie> movies)
    {
        foreach (var movie in movies)
        {
            Console.WriteLine($"ID: {movie.Id} Title: {movie.Title} Director: {movie.Director} Genre: {movie.Genre} Year: {movie.Year} Rating: {movie.Rating}");
            Console.WriteLine($"Actors: {string.Join(", ", movie.Actors)}");
        }
    }

    // New method to get actors
    public List<string> GetActors()
    {
        var actors = new List<string>();
        Console.WriteLine("Enter actors (type 'done' when finished):");

        while (true)
        {
            var actor = Console.ReadLine();
            if (string.Equals(actor, "done", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(actor))
            {
                break;
            }
            actors.Add(actor);
        }

        return actors;
    }
}
