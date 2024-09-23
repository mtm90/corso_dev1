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
            var input = _view.GetInput();
            if (input == "1")
            {
                AddMovie();
            }
            else if (input == "2")
            {
                ViewMovies();
            }
            else if (input == "3")
            {
                DeleteMovie();
            }
            else if (input == "4")
            {
                UpdateMovieTitle();
            }
            else if (input == "5")
            {
                _db.CloseConnection();
                break;
            }
        }
    }

    private void AddMovie()
    {
        Console.WriteLine("Enter the title of the movie");
        string title = _view.GetInput();
        Console.WriteLine("Enter the director of the movie");
        string director = _view.GetInput();
        Console.WriteLine("Enter the genre of the movie");
        string genre = _view.GetInput();
        Console.WriteLine("Enter the year of the movie");
        int year = int.Parse(_view.GetInput());
        Console.WriteLine("Enter the rating of the movie");
        int rating = int.Parse(_view.GetInput());

        // Get the list of actors using the new method
        var actors = _view.GetActors();

        _db.AddMovie(title, director, genre, year, rating, actors);
    }

    private void ViewMovies()
    {
        var movies = _db.GetMovies();
        _view.ShowMovies(movies);
    }

    private void DeleteMovie()
    {
        Console.WriteLine("Enter the title of the movie u want to delete");
        string title = _view.GetInput();

    _db.DeleteMovie(title);

    }
    private void UpdateMovieTitle()
    {
        Console.WriteLine("Enter the title of the movie u want to update");
        string oldTitle = _view.GetInput();
        Console.WriteLine("Enter the new Title");
        string newTitle = _view.GetInput();
        _db.UpdateMovieTitle(oldTitle, newTitle);



         
    }
}
