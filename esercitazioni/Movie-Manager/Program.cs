using Spectre.Console;

class Program
{
    static void Main()
    {
        Console.Clear();  // Clear the console at the start
        ShowProgramIntroduction();  // Display the program introduction

        string dbPath = "movieManager.db";  // Define the path for the SQLite database
        var dbContext = new DatabaseContext(dbPath);  // Initialize and connect to the SQLite database
        var movieView = new MovieView();  // View layer for movie-related operations
        var userView = new UserView();  // View layer for user-related operations
        var bookingView = new BookingView();  // View layer for booking-related operations
        var movieController = new MovieController(dbContext, movieView);  // Controller for movie operations
        var userController = new UserController(dbContext, userView);  // Controller for user operations
        var bookingController = new BookingController(dbContext, bookingView);  // Controller for booking operations

        // Main application loop to display the main menu and handle user choices
        while (true)
        {
            var mainChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()  // Create a selection prompt for the main menu
                    .Title("[blue]Main Menu[/]")  // Title of the main menu
                    .PageSize(4)  // Limit the displayed items to 4
                    .AddChoices(new[] { "Manage Movies", "Manage Users", "Manage Bookings", "Exit" }));  // Menu options

            // Handle user choices based on input
            switch (mainChoice)
            {
                case "Manage Movies":
                    ManageMovies(movieController);  // Call the method to manage movies
                    Console.Clear();  // Clear the console after returning from the submenu
                    break;
                case "Manage Users":
                    ManageUsers(userController);  // Call the method to manage users
                    Console.Clear();  // Clear the console after returning from the submenu
                    break;
                case "Manage Bookings":
                    ManageBookings(bookingController);  // Call the method to manage bookings
                    Console.Clear();  // Clear the console after returning from the submenu
                    break;
                case "Exit":
                    return;  // Exit the application
                default:
                    AnsiConsole.Markup("[red]Invalid choice. Please try again.[/]");  // Show error for invalid choice
                    break;
            }
        }
    }

    // Method to manage movies
    static void ManageMovies(MovieController movieController)
    {
        while (true)  // Loop for movie management options
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()  // Create a selection prompt for movie management
                    .Title("[gold3]Manage Movies[/]")  // Title of the movie management menu
                    .HighlightStyle(Color.Gold3)  // Highlight color for selected items
                    .PageSize(5)  // Limit displayed items to 5
                    .AddChoices(new[]  // Menu options for movie management
                    {
                        "Add a new movie",
                        "List all movies",
                        "Search movies",
                        "Order Movies by Duration",
                        "Back to Main Menu"
                    }));

            // Handle user choices based on input
            switch (choice)
            {
                case "Add a new movie":
                    movieController.AddMovie();  // Call method to add a new movie
                    break;
                case "List all movies":
                    movieController.ListAllMovies();  // Call method to list all movies
                    break;
                case "Search movies":
                    movieController.SearchMovies();  // Call method to search for movies
                    break;
                case "Order Movies by Duration":
                    movieController.OrderMoviesByDuration();  // Call method to order movies by duration
                    break;
                case "Back to Main Menu":
                    return;  // Return to the main menu
                default:
                    AnsiConsole.Markup("[red]Invalid choice. Please try again.[/]");  // Show error for invalid choice
                    break;
            }
        }
    }

    // Method to manage users
    static void ManageUsers(UserController userController)
    {
        while (true)  // Loop for user management options
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()  // Create a selection prompt for user management
                    .Title("[red]Manage Users[/]")  // Title of the user management menu
                    .HighlightStyle(Color.Red)  // Highlight color for selected items
                    .PageSize(3)  // Limit displayed items to 3
                    .AddChoices(new[]  // Menu options for user management
                    {
                        "Add a new user",
                        "List all users",
                        "Back to Main Menu"
                    }));

            // Handle user choices based on input
            switch (choice)
            {
                case "Add a new user":
                    userController.AddUser();  // Call method to add a new user
                    break;
                case "List all users":
                    userController.ListAllUsers();  // Call method to list all users
                    break;
                case "Back to Main Menu":
                    return;  // Return to the main menu
                default:
                    AnsiConsole.Markup("[red]Invalid choice. Please try again.[/]");  // Show error for invalid choice
                    break;
            }
        }
    }

    // Method to manage bookings
    static void ManageBookings(BookingController bookingController)
    {
        while (true)  // Loop for booking management options
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()  // Create a selection prompt for booking management
                    .Title("[violet]Manage Bookings[/]")  // Title of the booking management menu
                    .HighlightStyle(Color.Violet)  // Highlight color for selected items
                    .PageSize(5)  // Limit displayed items to 5
                    .AddChoices(new[]  // Menu options for booking management
                    {
                        "Create a booking",
                        "List all bookings",
                        "List all bookings with user name booking id booking date and movie title",
                        "Delete Booking",
                        "Update Booking",
                        "Back to Main Menu"
                    }));

            // Handle user choices based on input
            switch (choice)
            {
                case "Create a booking":
                    bookingController.AddBooking();  // Call method to create a new booking
                    break;
                case "List all bookings":
                    bookingController.ListAllBookings();  // Call method to list all bookings
                    break;
                case "List all bookings with user name booking id booking date and movie title":
                    bookingController.ListBookingsWithUserAndMovieDetails();  // Call method to list bookings with user details
                    break;
                case "Delete Booking":
                    bookingController.DeleteBooking();  // Call method to delete a booking
                    break;
                case "Update Booking":
                    bookingController.UpdateBooking();  // Call method to update a booking
                    break;
                case "Back to Main Menu":
                    return;  // Return to the main menu
                default:
                    AnsiConsole.Markup("[red]Invalid choice. Please try again.[/]");  // Show error for invalid choice
                    break;
            }
        }
    }

    // Method to display the program introduction
    static void ShowProgramIntroduction()
    {
        // Display a FigletText (ASCII Art) introduction
        AnsiConsole.Write(
            new FigletText("Movie Manager")
                .Centered()  // Center the text
                .Color(Color.Green));  // Set the text color to green

        // Display welcome messages
        AnsiConsole.MarkupLine("[bold yellow]Welcome to the Movie Manager Console Application![/]");  // Welcome message
        AnsiConsole.MarkupLine("[dim]This program helps you manage movies, users, and bookings easily.[/]");  // Description
        AnsiConsole.MarkupLine("[dim]Use the main menu to navigate through the available options.[/]");  // Instructions

        // Display a panel with instructions
        AnsiConsole.Write(
            new Panel("[bold]Press Enter to continue...[/]")  // Instructions inside the panel
                .Border(BoxBorder.Rounded)  // Rounded border for the panel
                .Header("[bold green]Let's Get Started![/]")  // Header for the panel
                .Expand());  // Expand the panel to fit content

        Console.ReadLine();  // Wait for the user to press Enter before continuing
    }
}
