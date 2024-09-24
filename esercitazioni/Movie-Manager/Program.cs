using Spectre.Console;
using System;

class Program
{
    static void Main()
    {
        string dbPath = "movieManager.db";  // Path to the SQLite database file
        var dbContext = new DatabaseContext(dbPath);
        var movieView = new MovieView();
        var userView = new UserView();
        var bookingView = new BookingView();
        var movieController = new MovieController(dbContext, movieView);
        var userController = new UserController(dbContext, userView);
        var bookingController = new BookingController(dbContext, bookingView);

        while (true)
        {
            // Create a Spectre.Console selection prompt
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Main Menu[/]")
                    .PageSize(10)  // Show up to 10 items at a time
                    .AddChoices(new[]
                    {
                        "Add a new movie",
                        "List all movies",
                        "Search movies",
                        "Add a new user",
                        "List all users",
                        "Create a booking",
                        "List all bookings",
                        "List all bookings with user details",
                        "Delete Booking",
                        "Update Booking",
                        "Exit"
                    }));

            // Use a switch statement to handle the user's choice
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
                case "Add a new user":
                    userController.AddUser();
                    break;
                case "List all users":
                    userController.ListAllUsers();
                    break;
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
                case "Exit":
                    return;  // Exit the program
                default:
                    AnsiConsole.Markup("[red]Invalid choice. Please try again.[/]");
                    break;
            }
        }
    }
}
