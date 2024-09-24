using Spectre.Console;

public class BookingView
{
    public (int UserId, int MovieId) GetBookingDetailsFromUser()
    {
        int userId = AnsiConsole.Ask<int>("Enter your [green]User ID[/]:");
        int movieId = AnsiConsole.Ask<int>("Enter [green]Movie ID[/] to book:");

        return (userId, movieId);
    }

    public void ShowBookingSuccess(Booking booking)
    {
        AnsiConsole.Markup($"[green]Booking ID {booking.BookingId}[/] created successfully for [blue]User ID {booking.UserId}[/] for [yellow]Movie ID {booking.MovieId}[/] on {booking.BookingDate}.");
    }

    public void DisplayBookings(List<Booking> bookings)
    {
        var table = new Table();
        table.AddColumn("Booking ID");
        table.AddColumn("User ID");
        table.AddColumn("Movie ID");
        table.AddColumn("Date");

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
