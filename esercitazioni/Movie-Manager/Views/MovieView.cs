public class MovieView
{
    public Movie GetMovieDetailsFromUser()
    {
        // Console input prompts acting as the View for user interaction
        Console.Write("Enter movie title: ");
        string title = Console.ReadLine();

        Console.Write("Enter movie genre: ");
        string genre = Console.ReadLine();

        Console.Write("Enter movie duration (in minutes): ");
        int duration;
        while (!int.TryParse(Console.ReadLine(), out duration) || duration <= 0)
        {
            Console.WriteLine("Please enter a valid duration in minutes.");
        }

        // Return a new movie object with user input
        return new Movie
        {
            Title = title,
            Genre = genre,
            Duration = duration
        };
    }

    public void ShowMovieAddedSuccess(Movie movie)
    {
        Console.WriteLine($"Movie '{movie.Title}' added successfully!");
    }

    public void DisplayMovies(List<Movie> movies)
    {
        Console.WriteLine("\nMovies in the database:");
        foreach (var movie in movies)
        {
            Console.WriteLine($"{movie.MovieId}: {movie.Title} ({movie.Genre}) - {movie.Duration} mins");
        }
    }
}
