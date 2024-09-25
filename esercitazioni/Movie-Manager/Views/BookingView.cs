// Handles user interaction for booking-related operations
using Spectre.Console; // Namespace for creating console applications with enhanced graphical elements

// View class responsible for managing user interactions related to bookings
public class BookingView
{
    // Collects booking details (user ID and movie ID) from the user
    public (int UserId, int MovieId) GetBookingDetailsFromUser()
    {
        // Prompt the user to enter their User ID.
        // This information will be used to associate the booking with the correct user in the system.
        int userId = AnsiConsole.Ask<int>("Enter [green]User ID[/]:"); // Get User ID from the user input.
        
        // Prompt the user to enter the Movie ID they wish to book.
        // This ID corresponds to a specific movie that the user wants to reserve.
        int movieId = AnsiConsole.Ask<int>("Enter [green]Movie ID[/] to book:"); // Get Movie ID from the user input.
        
        // Return the collected User ID and Movie ID as a tuple.
        // The tuple allows for easy passing of both values as a single return value.
        return (userId, movieId); // Return the user ID and movie ID as a tuple.
    }

    // Displays a success message for a newly created booking
    public void ShowBookingSuccess(Booking booking)
    {
        // Display a success message indicating that a booking has been created successfully.
        // The message includes the user ID, movie ID, and the date of the booking, formatted with colors for better readability.
        AnsiConsole.Markup($"[green]Booking created successfully for [blue]User ID {booking.UserId}[/] for [yellow]Movie ID {booking.MovieId}[/] on {booking.BookingDate}.[/]");
        
        // Add extra line breaks for visual spacing in the console output.
        // This improves user experience by separating messages and enhancing readability.
        Console.WriteLine(""); // Extra line for spacing
        Console.WriteLine(""); // Extra line for spacing
        Console.WriteLine(""); // Extra line for spacing
    }

    // Displays all bookings in a table format
    public void DisplayBookings(List<Booking> bookings)
    {
        // Create a new table instance to visually display the booking information.
        // This provides an organized way to present data to the user.
        var table = new Table(); // Create a new table instance
        
        // Define the columns for the table to represent the different attributes of each booking.
        table.AddColumn("Booking ID"); // Add column for Booking ID
        table.AddColumn("User ID");    // Add column for User ID
        table.AddColumn("Movie ID");   // Add column for Movie ID
        table.AddColumn("Date");       // Add column for Booking Date

        // Iterate over each booking in the provided list of bookings.
        // This allows us to populate the table with all existing booking records.
        foreach (var booking in bookings)
        {
            // Add a new row to the table for each booking, containing relevant details.
            // Each booking's information is converted to a string for display purposes.
            table.AddRow(
                booking.BookingId.ToString(), // Convert Booking ID to string for display
                booking.UserId.ToString(),     // Convert User ID to string for display
                booking.MovieId.ToString(),    // Convert Movie ID to string for display
                booking.BookingDate.ToString() // Convert Booking Date to string for display
            );
        }

        // Finally, display the constructed table in the console.
        // This renders the table with all the booking information to the user.
        AnsiConsole.Write(table); // Display the constructed table in the console.
    }
}
