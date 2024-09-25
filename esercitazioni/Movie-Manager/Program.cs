using Spectre.Console;

class Program
{
    static void Main()
    {
        Console.Clear();
        ShowProgramIntroduction();

        string dbPath = "movieManager.db";  
        var dbContext = new DatabaseContext(dbPath);  
        var movieView = new MovieView();  
        var userView = new UserView();  
        var bookingView = new BookingView();  
        var movieController = new MovieController(dbContext, movieView);  
        var userController = new UserController(dbContext, userView);  
        var bookingController = new BookingController(dbContext, bookingView);  

        while (true)
        {
            var mainChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]Main Menu[/]")
                    .PageSize(4)
                    .AddChoices(new[] { "Manage Movies", "Manage Users", "Manage Bookings", "Exit" }));

            switch (mainChoice)
            {
                case "Manage Movies":
                    ManageMovies(movieController);
                    Console.Clear();
                    break;
                case "Manage Users":
                    ManageUsers(userController);
                    Console.Clear();
                    break;
                case "Manage Bookings":
                    ManageBookings(bookingController);
                    Console.Clear();
                    break;
                case "Exit":
                    return;  
                default:
                    AnsiConsole.Markup("[red]Invalid choice. Please try again.[/]");
                    break;
            }
        }
    }

    // Method to manage movies
    static void ManageMovies(MovieController movieController)
    {
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[gold3]Manage Movies[/]")
                    .HighlightStyle(Color.Gold3)
                    .PageSize(5)
                    .AddChoices(new[]
                    {
                        "Add a new movie",
                        "List all movies",
                        "Search movies",
                        "Order Movies by Duration",
                        "Back to Main Menu"
                    }));

            switch (choice)
            {
                case "Add a new movie":
                    movieController.AddMovie();
                    break;
                case "List all movies":
                    movieController.ListAllMovies();
                    break;
                case "Search movies":
                    movieController.SearchMovies();
                    break;
                case "Order Movies by Duration":
                    movieController.OrderMoviesByDuration();
                    break;
                case "Back to Main Menu":
                    return;  
                default:
                    AnsiConsole.Markup("[red]Invalid choice. Please try again.[/]");
                    break;
            }
        }
    }

    // Method to manage users
    static void ManageUsers(UserController userController)
    {
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[red]Manage Users[/]")
                    .HighlightStyle(Color.Red)
                    .PageSize(3)
                    .AddChoices(new[]
                    {
                        "Add a new user",
                        "List all users",
                        "Back to Main Menu"
                    }));

            switch (choice)
            {
                case "Add a new user":
                    userController.AddUser();
                    break;
                case "List all users":
                    userController.ListAllUsers();
                    break;
                case "Back to Main Menu":
                    return;  
                default:
                    AnsiConsole.Markup("[red]Invalid choice. Please try again.[/]");
                    break;
            }
        }
    }

    // Method to manage bookings
    static void ManageBookings(BookingController bookingController)
    {
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[violet]Manage Bookings[/]")
                    .HighlightStyle(Color.Violet)
                    .PageSize(5)
                    .AddChoices(new[]
                    {
                        "Create a booking",
                        "List all bookings",
                        "List all bookings with user details",
                        "Delete Booking",
                        "Update Booking",
                        "Back to Main Menu"
                    }));

            switch (choice)
            {
                case "Create a booking":
                    bookingController.AddBooking();
                    break;
                case "List all bookings":
                    bookingController.ListAllBookings();
                    break;
                case "List all bookings with user details":
                    bookingController.ListBookingsWithUserDetails();
                    break;
                case "Delete Booking":
                    bookingController.DeleteBooking();
                    break;
                case "Update Booking":
                    bookingController.UpdateBooking();
                    break;
                case "Back to Main Menu":
                    return;  
                default:
                    AnsiConsole.Markup("[red]Invalid choice. Please try again.[/]");
                    break;
            }
        }
    }

    static void ShowProgramIntroduction()
    {
        AnsiConsole.Write(
            new FigletText("Movie Manager")
                .Centered()
                .Color(Color.Green));

        AnsiConsole.MarkupLine("[bold yellow]Welcome to the Movie Manager Console Application![/]");
        AnsiConsole.MarkupLine("[dim]This program helps you manage movies, users, and bookings easily.[/]");
        AnsiConsole.MarkupLine("[dim]Use the main menu to navigate through the available options.[/]");

        AnsiConsole.Write(
            new Panel("[bold]Press Enter to continue...[/]")
                .Border(BoxBorder.Rounded)
                .Header("[bold green]Let's Get Started![/]")
                .Expand());

        Console.ReadLine();
    }
}
