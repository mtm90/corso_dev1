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
        using var command = new System.Data.SQLite.SQLiteCommand(query, connection);
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
        using var command = new System.Data.SQLite.SQLiteCommand(query, connection);
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

    using var command = new System.Data.SQLite.SQLiteCommand(query, connection);
    using var reader = command.ExecuteReader();

    Console.WriteLine("Bookings with User Details:");
    while (reader.Read())
    {
        Console.WriteLine($"User ID: {reader["UserId"]}, Username: {reader["Username"]}, Booking ID: {reader["BookingId"]}, Movie ID: {reader["MovieId"]}, Date: {reader["BookingDate"]}");
    }
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
