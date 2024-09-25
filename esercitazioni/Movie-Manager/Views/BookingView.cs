// Handles user interaction for booking-related operations
using Spectre.Console;

public class BookingView
{
    // Collects booking details (user ID and movie ID) from the user
    public (int UserId, int MovieId) GetBookingDetailsFromUser()
    {
        int userId = AnsiConsole.Ask<int>("Enter [green]User ID[/]:");
        int movieId = AnsiConsole.Ask<int>("Enter [green]Movie ID[/] to book:");
        return (userId, movieId);
    }

    // Displays a success message for a newly created booking
    public void ShowBookingSuccess(Booking booking)
    {
        AnsiConsole.Markup($"[green]Booking ID {booking.BookingId}[/] created successfully for [blue]User ID {booking.UserId}[/] for [yellow]Movie ID {booking.MovieId}[/] on {booking.BookingDate}.");
    }

    // Displays all bookings in a table format
    public void DisplayBookings(List<Booking> bookings)
    {
        var table = new Table();
        table.AddColumn("Booking ID");
        table.AddColumn("User ID");
        table.AddColumn("Movie ID");
        table.AddColumn("Date");

        // Add each booking's data to the table
        foreach (var booking in bookings)
        {
            table.AddRow(
                booking.BookingId.ToString(),
                booking.UserId.ToString(),
                booking.MovieId.ToString(),
                booking.BookingDate.ToString()
            );
        }

        AnsiConsole.Write(table);
    }
}