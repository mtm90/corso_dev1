// Handles all booking-related operations (add, list, update, delete bookings)
using System.Data.SQLite;  // Namespace for SQLite database functionality
using Spectre.Console;    // Namespace for console output styling

public class BookingController
{
    private readonly DatabaseContext _dbContext; // Instance of DatabaseContext to interact with the database
    private readonly BookingView _view;           // Instance of BookingView for user interface interactions

    // Constructor to initialize BookingController with the database context and view
    public BookingController(DatabaseContext dbContext, BookingView view)
    {
        _dbContext = dbContext; // Assign the provided DatabaseContext to the private field
        _view = view;           // Assign the provided BookingView to the private field
    }

    // Adds a new booking for a specified user and movie.
    // It first retrieves the user ID and movie ID from the user input via the BookingView.
    // Then it validates the existence of the user and movie in the database.
    // If both exist, it creates a new Booking object with the current date and time.
    // The method constructs and executes an SQL INSERT command to add the booking to the database.
    // Finally, it updates the movie's status to booked and displays a success message.
    public void AddBooking()
    {
        // Get booking details (user ID and movie ID) from the user input through the view
        var (userId, movieId) = _view.GetBookingDetailsFromUser();

        // Check if the user and movie exist; if not, inform the user and exit the method
        if (!UserExists(userId) || !MovieExists(movieId))
        {
            Console.WriteLine("Invalid User ID or Movie ID. Booking could not be created.");
            return; // Exit the method if validation fails
        }

        using var connection = _dbContext.GetConnection(); // Get a connection to the database
        connection.Open(); // Open the database connection

        // Create a new Booking object with user ID, movie ID, and current date/time as booking date
        var newBooking = new Booking
        {
            UserId = userId,
            MovieId = movieId,
            BookingDate = DateTime.Now // Set the booking date to the current date and time
        };

        // SQL query to insert a new booking into the Bookings table
        string insertBookingQuery = "INSERT INTO Bookings (UserId, MovieId, BookingDate) VALUES (@UserId, @MovieId, @BookingDate)";
        
        using var insertCommand = new SQLiteCommand(insertBookingQuery, connection); // Prepare the SQL command
        // Bind parameters to prevent SQL injection
        insertCommand.Parameters.AddWithValue("@UserId", newBooking.UserId);
        insertCommand.Parameters.AddWithValue("@MovieId", newBooking.MovieId);
        insertCommand.Parameters.AddWithValue("@BookingDate", newBooking.BookingDate);
        insertCommand.ExecuteNonQuery(); // Execute the insert command

        // SQL query to update the movie's booking status to booked
        string updateMovieQuery = "UPDATE Movies SET IsBooked = 1 WHERE MovieId = @MovieId";
        using var updateCommand = new SQLiteCommand(updateMovieQuery, connection); // Prepare the update command
        updateCommand.Parameters.AddWithValue("@MovieId", movieId); // Bind the movie ID parameter
        updateCommand.ExecuteNonQuery(); // Execute the update command to set the movie as booked

        _view.ShowBookingSuccess(newBooking); // Display a success message to the user through the view
    }

    // Lists all bookings currently stored in the database.
    // It establishes a connection to the database, executes an SQL SELECT command
    // to retrieve all booking records, and iterates through the results.
    // Each booking record is added to a list, which is then passed to the BookingView
    // to display the details of all bookings to the user.   
    public void ListAllBookings()
    {
        var bookings = new List<Booking>(); // Initialize a list to store bookings

        using var connection = _dbContext.GetConnection(); // Get a connection to the database
        connection.Open(); // Open the database connection

        // SQL query to select all bookings from the Bookings table
        string query = "SELECT BookingId, UserId, MovieId, BookingDate FROM Bookings";
        using var command = new SQLiteCommand(query, connection); // Prepare the SQL command
        using var reader = command.ExecuteReader(); // Execute the command and get a data reader

        // Read each booking record and add it to the bookings list
        while (reader.Read())
        {
            bookings.Add(new Booking
            {
                BookingId = Convert.ToInt32(reader["BookingId"]), // Convert the booking ID to int and store
                UserId = Convert.ToInt32(reader["UserId"]),       // Convert the user ID to int and store
                MovieId = Convert.ToInt32(reader["MovieId"]),     // Convert the movie ID to int and store
                BookingDate = Convert.ToDateTime(reader["BookingDate"]) // Convert the booking date to DateTime and store
            });
        }

        // Display bookings using the view
        _view.DisplayBookings(bookings); // Call the view method to show the bookings
    }

    // Displays bookings along with the associated user and movie details.
    // It establishes a connection to the database and executes an SQL JOIN query
    // to retrieve booking information along with usernames and movie titles.
    // A table is created to format the output, and each result row is added
    // to the table for display. Finally, the formatted table is presented to the user.    
    public void ListBookingsWithUserAndMovieDetails()
    {
        using var connection = _dbContext.GetConnection(); // Get a connection to the database
        connection.Open(); // Open the database connection

        // SQL query to join Users, Bookings, and Movies to get relevant details
        string query = @"
        SELECT 
            u.Username, b.BookingId, m.Title, b.BookingDate 
        FROM Users u
        INNER JOIN Bookings b ON u.UserId = b.UserId
        INNER JOIN Movies m ON b.MovieId = m.MovieId;";

        using var command = new SQLiteCommand(query, connection); // Prepare the SQL command
        using var reader = command.ExecuteReader(); // Execute the command and get a data reader

        var table = new Table(); // Create a new table to display results
        // Define columns for the table
        table.AddColumn("Booking ID");
        table.AddColumn("Username");
        table.AddColumn("Movie Title");
        table.AddColumn("Booking Date");

        // Populate the table with booking data and display it
        while (reader.Read())
        {
            table.AddRow(
                reader["BookingId"].ToString(), // Add booking ID to the table
                reader["Username"].ToString(),   // Add username to the table
                reader["Title"].ToString(),      // Add movie title to the table
                reader["BookingDate"].ToString() // Add booking date to the table
            );
        }

        AnsiConsole.Write(table); // Display the populated table in the console
    }

    // Deletes a booking from the database based on the provided booking ID.
    // The method prompts the user for a booking ID and checks if it is valid
    // by attempting to parse it into an integer. If the booking exists,
    // it constructs and executes an SQL DELETE command to remove the booking.
    // The user is informed of the success or failure of the deletion operation.    
    public void DeleteBooking()
    {
        Console.Write("Enter Booking ID to delete: "); // Prompt the user for the booking ID
        if (int.TryParse(Console.ReadLine(), out int bookingId)) // Try to parse user input as an integer
        {
            if (!BookingExists(bookingId)) // Check if the booking exists
            {
                AnsiConsole.WriteLine("Booking ID not found."); // Inform the user if the booking ID is not found
                return; // Exit the method if the booking does not exist
            }

            using var connection = _dbContext.GetConnection(); // Get a connection to the database
            connection.Open(); // Open the database connection

            // SQL query to delete the booking from the Bookings table
            string deleteQuery = "DELETE FROM Bookings WHERE BookingId = @BookingId";
            using var deleteCommand = new SQLiteCommand(deleteQuery, connection); // Prepare the delete command
            deleteCommand.Parameters.AddWithValue("@BookingId", bookingId); // Bind the booking ID parameter
            int affectedRows = deleteCommand.ExecuteNonQuery(); // Execute the delete command

            // Notify user if deletion was successful
            if (affectedRows > 0)
            {
                Console.WriteLine("Booking deleted successfully."); // Inform user of successful deletion
            }
            else
            {
                Console.WriteLine("Failed to delete the booking."); // Inform user if deletion failed
            }
        }
        else
        {
            Console.WriteLine("Invalid Booking ID. Please enter a valid numeric value."); // Inform user of invalid input
        }
    }

    // Updates an existing booking in the database, allowing the user to change the movie and booking date.
    // The method prompts the user for a booking ID, retrieves the current booking details,
    // and allows the user to select a new movie. It then updates the booking in the database.
    // A confirmation message is displayed upon successful update
    public void UpdateBooking()
    {
        Console.Write("Enter Booking ID to update: "); // Prompt the user for the booking ID
        if (int.TryParse(Console.ReadLine(), out int bookingId)) // Try to parse user input as an integer
        {
            if (!BookingExists(bookingId)) // Check if the booking exists
            {
                Console.WriteLine("Booking ID not found."); // Inform the user if the booking ID is not found
                return; // Exit the method if the booking does not exist
            }

            // Collect new details for the booking
            Console.Write("Enter new Movie ID: "); // Prompt user for new movie ID
            if (!int.TryParse(Console.ReadLine(), out int newMovieId)) // Try to parse new movie ID
            {
                Console.WriteLine("Invalid Movie ID."); // Inform user if the input is invalid
                return; // Exit the method if the input is invalid
            }

            Console.Write("Enter new Booking Date (yyyy-mm-dd): "); // Prompt user for new booking date
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime newBookingDate)) // Try to parse new booking date
            {
                Console.WriteLine("Invalid Date format."); // Inform user if the input is invalid
                return; // Exit the method if the input is invalid
            }

            using var connection = _dbContext.GetConnection(); // Get a connection to the database
            connection.Open(); // Open the database connection

            // SQL query to update the booking details in the Bookings table
            string updateQuery = "UPDATE Bookings SET MovieId = @MovieId, BookingDate = @BookingDate WHERE BookingId = @BookingId";
            using var updateCommand = new SQLiteCommand(updateQuery, connection); // Prepare the update command
            updateCommand.Parameters.AddWithValue("@MovieId", newMovieId); // Bind the new movie ID parameter
            updateCommand.Parameters.AddWithValue("@BookingDate", newBookingDate); // Bind the new booking date parameter
            updateCommand.Parameters.AddWithValue("@BookingId", bookingId); // Bind the booking ID parameter

            int affectedRows = updateCommand.ExecuteNonQuery(); // Execute the update command

            // Notify user if the update was successful
            if (affectedRows > 0)
            {
                Console.WriteLine("Booking updated successfully."); // Inform user of successful update
            }
            else
            {
                Console.WriteLine("Failed to update the booking."); // Inform user if the update failed
            }
        }
        else
        {
            Console.WriteLine("Invalid Booking ID. Please enter a valid numeric value."); // Inform user of invalid input
        }
    }

    // Helper method to check if a booking exists in the database
    private bool BookingExists(int bookingId)
    {
        using var connection = _dbContext.GetConnection(); // Get a connection to the database
        connection.Open(); // Open the database connection

        // SQL query to count the number of bookings with the specified booking ID
        string query = "SELECT COUNT(*) FROM Bookings WHERE BookingId = @BookingId";
        using var command = new SQLiteCommand(query, connection); // Prepare the SQL command
        command.Parameters.AddWithValue("@BookingId", bookingId); // Bind the booking ID parameter

        // Return true if at least one booking with the specified ID exists; otherwise, return false
        return Convert.ToInt32(command.ExecuteScalar()) > 0;
    }

    // Helper method to check if a user exists in the database
    private bool UserExists(int userId)
    {
        using var connection = _dbContext.GetConnection(); // Get a connection to the database
        connection.Open(); // Open the database connection

        // SQL query to count the number of users with the specified user ID
        string query = "SELECT COUNT(*) FROM Users WHERE UserId = @UserId";
        using var command = new SQLiteCommand(query, connection); // Prepare the SQL command
        command.Parameters.AddWithValue("@UserId", userId); // Bind the user ID parameter

        // Return true if at least one user with the specified ID exists; otherwise, return false
        return Convert.ToInt32(command.ExecuteScalar()) > 0;
    }

    // Helper method to check if a movie exists in the database
    private bool MovieExists(int movieId)
    {
        using var connection = _dbContext.GetConnection(); // Get a connection to the database
        connection.Open(); // Open the database connection

        // SQL query to count the number of movies with the specified movie ID
        string query = "SELECT COUNT(*) FROM Movies WHERE MovieId = @MovieId";
        using var command = new SQLiteCommand(query, connection); // Prepare the SQL command
        command.Parameters.AddWithValue("@MovieId", movieId); // Bind the movie ID parameter

        // Return true if at least one movie with the specified ID exists; otherwise, return false
        return Convert.ToInt32(command.ExecuteScalar()) > 0;
    }
}
