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

    public void AddBooking()
    {
        // Get booking details from the view
        var (userId, movieId) = _view.GetBookingDetailsFromUser();

        // Ensure user and movie exist before proceeding
        if (!UserExists(userId) || !MovieExists(movieId))
        {
            Console.WriteLine("Invalid User ID or Movie ID. Booking could not be created.");
            return;
        }

        using var connection = _dbContext.GetConnection();
        connection.Open();

        // Create a new booking
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
        newBooking.BookingId = (int)connection.LastInsertRowId;

        // Notify the view that the booking was added successfully
        _view.ShowBookingSuccess(newBooking);
    }

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

        // Display all bookings using the view
        _view.DisplayBookings(bookings);
    }


    public void ListBookingsWithUserDetails()
{
    using var connection = _dbContext.GetConnection();
    connection.Open();

    string query = @"
        SELECT 
            u.UserId, 
            u.Username, 
            b.BookingId, 
            b.MovieId, 
            b.BookingDate 
        FROM 
            Users u
        INNER JOIN 
            Bookings b ON u.UserId = b.UserId;";

    using var command = new SQLiteCommand(query, connection);
    using var reader = command.ExecuteReader();

    // Create a table for displaying booking information
    var table = new Table();
    table.AddColumn("User ID");
    table.AddColumn("Username");
    table.AddColumn("Booking ID");
    table.AddColumn("Movie ID");
    table.AddColumn("Booking Date");

    // Populate the table with data
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

    // Display the table using Spectre.Console
    AnsiConsole.Write(table);
}


    public void DeleteBooking()
{
    Console.Write("Enter Booking ID to delete: ");
    if (int.TryParse(Console.ReadLine(), out int bookingId))
    {
        if (!BookingExists(bookingId))
        {
            Console.WriteLine("Booking ID not found.");
            return;
        }

        try
        {
            using var connection = _dbContext.GetConnection();
            connection.Open();

            // Delete the booking from the database
            string deleteQuery = "DELETE FROM Bookings WHERE BookingId = @BookingId";
            using var deleteCommand = new SQLiteCommand(deleteQuery, connection);
            deleteCommand.Parameters.AddWithValue("@BookingId", bookingId);
            int affectedRows = deleteCommand.ExecuteNonQuery();

            if (affectedRows > 0)
            {
                Console.WriteLine("Booking deleted successfully.");
            }
            else
            {
                Console.WriteLine("Failed to delete the booking.");
            }
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Database error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }
    else
    {
        Console.WriteLine("Invalid Booking ID. Please enter a valid numeric value.");
    }
}

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

        try
        {
            using var connection = _dbContext.GetConnection();
            connection.Open();

            // Update the booking in the database
            string updateQuery = "UPDATE Bookings SET MovieId = @MovieId, BookingDate = @BookingDate WHERE BookingId = @BookingId";
            using var updateCommand = new System.Data.SQLite.SQLiteCommand(updateQuery, connection);
            updateCommand.Parameters.AddWithValue("@MovieId", newMovieId);
            updateCommand.Parameters.AddWithValue("@BookingDate", newBookingDate);
            updateCommand.Parameters.AddWithValue("@BookingId", bookingId);

            int affectedRows = updateCommand.ExecuteNonQuery();

            if (affectedRows > 0)
            {
                Console.WriteLine("Booking updated successfully.");
            }
            else
            {
                Console.WriteLine("Failed to update the booking.");
            }
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Database error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }
    else
    {
        Console.WriteLine("Invalid Booking ID. Please enter a valid numeric value.");
    }
}


    private bool BookingExists(int bookingId)
{
    using var connection = _dbContext.GetConnection();
    connection.Open();

    string query = "SELECT COUNT(*) FROM Bookings WHERE BookingId = @BookingId";
    using var command = new SQLiteCommand(query, connection);
    command.Parameters.AddWithValue("@BookingId", bookingId);

    return Convert.ToInt32(command.ExecuteScalar()) > 0;
}



    private bool UserExists(int userId)
    {
        using var connection = _dbContext.GetConnection();
        connection.Open();

        string query = "SELECT COUNT(*) FROM Users WHERE UserId = @UserId";
        using var command = new System.Data.SQLite.SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@UserId", userId);

        return Convert.ToInt32(command.ExecuteScalar()) > 0;
    }

    private bool MovieExists(int movieId)
    {
        using var connection = _dbContext.GetConnection();
        connection.Open();

        string query = "SELECT COUNT(*) FROM Movies WHERE MovieId = @MovieId";
        using var command = new System.Data.SQLite.SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@MovieId", movieId);

        return Convert.ToInt32(command.ExecuteScalar()) > 0;
    }
}
