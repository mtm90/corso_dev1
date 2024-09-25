// Handles user interaction for booking-related operations
using Spectre.Console; // Namespace for creating console applications with enhanced graphical elements

// View class responsible for managing user interactions related to bookings
public class BookingView
{
    // Collects booking details (user ID and movie ID) from the user
    public (int UserId, int MovieId) GetBookingDetailsFromUser()
    {
        // Prompt the user to enter their User ID and Movie ID for booking
        int userId = AnsiConsole.Ask<int>("Enter [green]User ID[/]:"); // Get User ID
        int movieId = AnsiConsole.Ask<int>("Enter [green]Movie ID[/] to book:"); // Get Movie ID
        return (userId, movieId); // Return the user ID and movie ID as a tuple
    }

    // Displays a success message for a newly created booking
    public void ShowBookingSuccess(Booking booking)
    {
        // Display a success message with booking details using markup for color
        AnsiConsole.Markup($"[green]Booking created successfully for [blue]User ID {booking.UserId}[/] for [yellow]Movie ID {booking.MovieId}[/] on {booking.BookingDate}.");
        Console.WriteLine(""); // Extra line for spacing
        Console.WriteLine(""); // Extra line for spacing
        Console.WriteLine(""); // Extra line for spacing
    }

    // Displays all bookings in a table format
    public void DisplayBookings(List<Booking> bookings)
    {
        var table = new Table(); // Create a new table instance
        table.AddColumn("Booking ID"); // Add column for Booking ID
        table.AddColumn("User ID"); // Add column for User ID
        table.AddColumn("Movie ID"); // Add column for Movie ID
        table.AddColumn("Date"); // Add column for Booking Date

        // Add each booking's data to the table
        foreach (var booking in bookings)
        {
            // Add a row for each booking with relevant details
            table.AddRow(
                booking.BookingId.ToString(), // Convert Booking ID to string
                booking.UserId.ToString(),     // Convert User ID to string
                booking.MovieId.ToString(),    // Convert Movie ID to string
                booking.BookingDate.ToString() // Convert Booking Date to string
            );
        }

        // Display the constructed table in the console
        AnsiConsole.Write(table);
    }
}
