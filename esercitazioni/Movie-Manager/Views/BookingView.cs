public class BookingView
{
   public (int UserId, int MovieId) GetBookingDetailsFromUser()
{
    int userId = 0;
    int movieId = 0;
    
    while (true)
    {
        try
        {
            Console.Write("Enter your User ID: ");
            userId = int.Parse(Console.ReadLine());

            Console.Write("Enter Movie ID to book: ");
            movieId = int.Parse(Console.ReadLine());
            break; // Exit the loop if parsing is successful
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid input. Please enter numeric values for User ID and Movie ID.");
        }
        catch (OverflowException)
        {
            Console.WriteLine("Input is too large. Please enter a valid User ID and Movie ID.");
        }
    }

    return (userId, movieId);
}

    public void ShowBookingSuccess(Booking booking)
    {
        Console.WriteLine($"Booking ID {booking.BookingId} created successfully for User ID {booking.UserId} for Movie ID {booking.MovieId} on {booking.BookingDate}.");
    }

    public void DisplayBookings(List<Booking> bookings)
    {
        Console.WriteLine("\nBookings in the database:");
        foreach (var booking in bookings)
        {
            Console.WriteLine($"Booking ID: {booking.BookingId}, User ID: {booking.UserId}, Movie ID: {booking.MovieId}, Date: {booking.BookingDate}");
        }
    }
}
