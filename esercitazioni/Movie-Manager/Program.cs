using Spectre.Console;

class Program
{
    static void Main()
    {
        Console.Clear();
        // Call the introduction method to display the program introduction
        ShowProgramIntroduction();

        string dbPath = "movieManager.db";  // Path to the SQLite database file
        var dbContext = new DatabaseContext(dbPath);  // Initializes and connects to the SQLite database
        var movieView = new MovieView();  // View layer for movies
        var userView = new UserView();  // View layer for users
        var bookingView = new BookingView();  // View layer for bookings
        var movieController = new MovieController(dbContext, movieView);  // Controller for movie operations
        var userController = new UserController(dbContext, userView);  // Controller for user operations
        var bookingController = new BookingController(dbContext, bookingView);  // Controller for booking operations

        // Main application loop to display a menu and handle user choices
        while (true)
        {   
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Main Menu[/]")  // Main menu title
                    .PageSize(10)  // Limit displayed items to 10
                    .AddChoices(new[]
                    {
                        "Add a new movie", "List all movies", "Search movies",
                        "Add a new user", "List all users", "Create a booking",
                        "List all bookings", "List all bookings with user details",
                        "Delete Booking", "Update Booking", "Exit"
                    }));

            // Handle menu choices based on user input
            switch (choice)
            {
                case "Add a new movie":
                    movieController.AddMovie();  // Add a new movie
                    break;
                case "List all movies":
                    movieController.ListAllMovies();  // List all movies
                    break;
                case "Search movies":
                    movieController.SearchMovies();  // Search for movies
                    break;
                case "Add a new user":
                    userController.AddUser();  // Add a new user
                    break;
                case "List all users":
                    userController.ListAllUsers();  // List all users
                    break;
                case "Create a booking":
                    bookingController.AddBooking();  // Create a new booking
                    break;
                case "List all bookings":
                    bookingController.ListAllBookings();  // List all bookings
                    break;
                case "List all bookings with user details":
                    bookingController.ListBookingsWithUserDetails();  // List bookings with user details
                    break;
                case "Delete Booking":
                    bookingController.DeleteBooking();  // Delete a booking
                    break;
                case "Update Booking":
                    bookingController.UpdateBooking();  // Update a booking
                    break;
                case "Exit":
                    return;  // Exit the application
                default:
                    AnsiConsole.Markup("[red]Invalid choice. Please try again.[/]");
                    break;
            }
        }
    }

    // Method to display the program introduction using Spectre.Console elements
    static void ShowProgramIntroduction()
    {
        // Display a FigletText (ASCII Art) introduction
        AnsiConsole.Write(
            new FigletText("Movie Manager")
                .Centered()
                .Color(Color.Green));

        // Display some description text with animations
        AnsiConsole.MarkupLine("[bold yellow]Welcome to the Movie Manager Console Application![/]");
        AnsiConsole.MarkupLine("[dim]This program helps you manage movies, users, and bookings easily.[/]");
        AnsiConsole.MarkupLine("[dim]Use the main menu to navigate through the available options.[/]");

        // Display a box with additional instructions
        AnsiConsole.Write(
            new Panel("[bold]Press Enter to continue...[/]")
                .Border(BoxBorder.Rounded)
                .Header("[bold green]Let's Get Started![/]")
                .Expand());

        // Wait for the user to press Enter before continuing
        Console.ReadLine();
    }
}
