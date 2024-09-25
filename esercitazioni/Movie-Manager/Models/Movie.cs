public class Movie
{
    public int MovieId { get; set; } // Unique identifier for the Movie
    public string Title { get; set; } // Title of the Movie
    public string Genre { get; set; } // Genre of the Movie
    public int Duration { get; set; } // Duration of the Movie in minutes
    public bool IsBooked { get; set; }  // Bool value that tells if the movie i already booked or not
}
