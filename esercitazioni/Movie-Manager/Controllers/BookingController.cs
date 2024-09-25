// Handles all booking-related operations (add, list, update, delete bookings)
using System.Data.SQLite;
using Spectre.Console;

public class BookingController
{
    private readonly DatabaseContext _dbContext;
    private readonly BookingView _view;

    public BookingController(DatabaseContext dbContext, BookingView view)
    {
        _dbContext = dbContext;
        _view = view;
    }

    // Adds a new booking, ensuring the movie and user exist before creating the booking
    public void AddBooking()
    {
        var (userId, movieId) = _view.GetBookingDetailsFromUser();  // Get booking details from user input

        // Check if user and movie exist before proceeding
        if (!UserExists(userId) || !MovieExists(movieId))
        {
            Console.WriteLine("Invalid User ID or Movie ID. Booking could not be created.");
            return;
        }

        using var connection = _dbContext.GetConnection();
        connection.Open();

        var newBooking = new Booking
        {
            UserId = userId,
            MovieId = movieId,
            BookingDate = DateTime.Now
        };

        string query = "INSERT INTO Bookings (UserId, MovieId, BookingDate) VALUES (@UserId, @MovieId, @BookingDate)";
        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@UserId", newBooking.UserId);
        command.Parameters.AddWithValue("@MovieId", newBooking.MovieId);
        command.Parameters.AddWithValue("@BookingDate", newBooking.BookingDate);
        command.ExecuteNonQuery();

        newBooking.BookingId = (int)connection.LastInsertRowId;  // Retrieve the new booking ID
        _view.ShowBookingSuccess(newBooking);  // Notify the view of the successful booking creation
    }

    // Lists all bookings from the database
    public void ListAllBookings()
    {
        var bookings = new List<Booking>();

        using var connection = _dbContext.GetConnection();
        connection.Open();

        string query = "SELECT BookingId, UserId, MovieId, BookingDate FROM Bookings";
        using var command = new SQLiteCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            bookings.Add(new Booking
            {
                BookingId = Convert.ToInt32(reader["BookingId"]),
                UserId = Convert.ToInt32(reader["UserId"]),
                MovieId = Convert.ToInt32(reader["MovieId"]),
                BookingDate = Convert.ToDateTime(reader["BookingDate"])
            });
        }

        // Display bookings using the view
        _view.DisplayBookings(bookings);
    }

    // Displays bookings along with the user details
    public void ListBookingsWithUserDetails()
    {
        using var connection = _dbContext.GetConnection();
        connection.Open();

        string query = @"
            SELECT 
                u.UserId, u.Username, b.BookingId, b.MovieId, b.BookingDate 
            FROM Users u
            INNER JOIN Bookings b ON u.UserId = b.UserId;";

        using var command = new SQLiteCommand(query, connection);
        using var reader = command.ExecuteReader();

        var table = new Table();
        table.AddColumn("User ID");
        table.AddColumn("Username");
        table.AddColumn("Booking ID");
        table.AddColumn("Movie ID");
        table.AddColumn("Booking Date");

        // Populate table with booking data and display it
        while (reader.Read())
        {
            table.AddRow(
                reader["UserId"].ToString(),
                reader["Username"].ToString(),
                reader["BookingId"].ToString(),
                reader["MovieId"].ToString(),
                reader["BookingDate"].ToString()
            );
        }

        AnsiConsole.Write(table);
    }

    // Deletes a booking based on its ID
    public void DeleteBooking()
    {
        Console.Write("Enter Booking ID to delete: ");
        if (int.TryParse(Console.ReadLine(), out int bookingId))
        {
            if (!BookingExists(bookingId))
            {
                AnsiConsole.WriteLine("Booking ID not found.");
                return;
            }

            using var connection = _dbContext.GetConnection();
            connection.Open();

            string deleteQuery = "DELETE FROM Bookings WHERE BookingId = @BookingId";
            using var deleteCommand = new SQLiteCommand(deleteQuery, connection);
            deleteCommand.Parameters.AddWithValue("@BookingId", bookingId);
            int affectedRows = deleteCommand.ExecuteNonQuery();

            // Notify user if deletion was successful
            if (affectedRows > 0)
            {
                Console.WriteLine("Booking deleted successfully.");
            }
            else
            {
                Console.WriteLine("Failed to delete the booking.");
            }
        }
        else
        {
            Console.WriteLine("Invalid Booking ID. Please enter a valid numeric value.");
        }
    }

    // Updates a booking by allowing the user to change movie and booking date
    public void UpdateBooking()
    {
        Console.Write("Enter Booking ID to update: ");
        if (int.TryParse(Console.ReadLine(), out int bookingId))
        {
            if (!BookingExists(bookingId))
            {
                Console.WriteLine("Booking ID not found.");
                return;
            }

            // Collect new details for the booking
            Console.Write("Enter new Movie ID: ");
            if (!int.TryParse(Console.ReadLine(), out int newMovieId))
            {
                Console.WriteLine("Invalid Movie ID.");
                return;
            }

            Console.Write("Enter new Booking Date (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime newBookingDate))
            {
                Console.WriteLine("Invalid Date format.");
                return;
            }

            using var connection = _dbContext.GetConnection();
            connection.Open();

            string updateQuery = "UPDATE Bookings SET MovieId = @MovieId, BookingDate = @BookingDate WHERE BookingId = @BookingId";
            using var updateCommand = new SQLiteCommand(updateQuery, connection);
            updateCommand.Parameters.AddWithValue("@MovieId", newMovieId);
            updateCommand.Parameters.AddWithValue("@BookingDate", newBookingDate);
            updateCommand.Parameters.AddWithValue("@BookingId", bookingId);

            int affectedRows = updateCommand.ExecuteNonQuery();

            // Notify user if the update was successful
            if (affectedRows > 0)
            {
                Console.WriteLine("Booking updated successfully.");
            }
            else
            {
                Console.WriteLine("Failed to update the booking.");
            }
        }
        else
        {
            Console.WriteLine("Invalid Booking ID. Please enter a valid numeric value.");
        }
    }

    // Helper method to check if a booking exists in the database
    private bool BookingExists(int bookingId)
    {
        using var connection = _dbContext.GetConnection();
        connection.Open();

        string query = "SELECT COUNT(*) FROM Bookings WHERE BookingId = @BookingId";
        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@BookingId", bookingId);

        return Convert.ToInt32(command.ExecuteScalar()) > 0;
    }

    // Helper method to check if a user exists in the database
    private bool UserExists(int userId)
    {
        using var connection = _dbContext.GetConnection();
        connection.Open();

        string query = "SELECT COUNT(*) FROM Users WHERE UserId = @UserId";
        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@UserId", userId);

        return Convert.ToInt32(command.ExecuteScalar()) > 0;
    }

    // Helper method to check if a movie exists in the database
    private bool MovieExists(int movieId)
    {
        using var connection = _dbContext.GetConnection();
        connection.Open();

        string query = "SELECT COUNT(*) FROM Movies WHERE MovieId = @MovieId";
        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@MovieId", movieId);

        return Convert.ToInt32(command.ExecuteScalar()) > 0;
    }
}
