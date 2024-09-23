class Movie
{
    public int Id {get;set;}
    public string Title {get;set;}
    public string Director {get;set;}
    public string Genre  {get;set;}
    public int Year {get;set;}
    public int Rating {get;set;}

    public List<string> Actors {get;set;}

    public Movie(int id, string title, string director, string genre, int year, int rating, List<string> actors)
    {
        Id = id;
        Title = title;
        Director = director;
        Genre = genre;
        Year = year;
        Rating = rating;
        Actors = actors;
    }
}