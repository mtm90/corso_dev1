using System;
using Spectre.Console;

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
            Console.WriteLine("\nMain Menu:");
            Console.WriteLine("1. Add a new movie");
            Console.WriteLine("2. List all movies");
            Console.WriteLine("3. Add a new user");
            Console.WriteLine("4. List all users");
            Console.WriteLine("5. Create a booking");
            Console.WriteLine("6. List all bookings");
            Console.WriteLine("7. List all bookings with user details");
            Console.WriteLine("8. Delete Booking");
            Console.WriteLine("9. Exit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    movieController.AddMovie();
                    break;
                case "2":
                    movieController.ListAllMovies();
                    break;
                case "3":
                    userController.AddUser();
                    break;
                case "4":
                    userController.ListAllUsers();
                    break;
                case "5":
                    bookingController.AddBooking();
                    break;
                case "6":
                    bookingController.ListAllBookings();
                    break;
                case "7":
                    bookingController.ListBookingsWithUserDetails();
                    break;
                case "8":
                    bookingController.DeleteBooking();
                    break;
                case "9":
                    return;  // Exit the program
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}
