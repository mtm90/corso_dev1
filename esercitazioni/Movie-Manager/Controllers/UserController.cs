// Handles operations related to users such as adding and listing users
using System.Data.SQLite; // Namespace for SQLite database functionality

// Controller class responsible for managing user-related operations
public class UserController
{
    private readonly DatabaseContext _dbContext; // Instance of DatabaseContext for database interactions
    private readonly UserView _view;               // Instance of UserView for user interface interactions

    // Constructor to initialize UserController with the database context and view
    public UserController(DatabaseContext dbContext, UserView view)
    {
        _dbContext = dbContext; // Assign the provided DatabaseContext to the private field
        _view = view;           // Assign the provided UserView to the private field
    }

    // Adds a new user to the database
    public void AddUser()
    {
        // Get user details from the view through user input
        User newUser = _view.GetUserDetailsFromUser();

        using var connection = _dbContext.GetConnection(); // Get a connection to the database
        connection.Open(); // Open the database connection

        // SQL query to insert a new user into the Users table
        string query = "INSERT INTO Users (Username, Email, Password) VALUES (@Username, @Email, @Password)";
        using var command = new SQLiteCommand(query, connection); // Prepare the SQL command
        // Bind parameters to prevent SQL injection
        command.Parameters.AddWithValue("@Username", newUser.Username);
        command.Parameters.AddWithValue("@Email", newUser.Email);
        command.Parameters.AddWithValue("@Password", newUser.Password); // Note: Password hashing recommended for security
        command.ExecuteNonQuery(); // Execute the insert command

        // Notify the view of successful user addition
        _view.ShowUserAddedSuccess(newUser); // Display success message to the user
    }

    // Lists all users from the database
    public void ListAllUsers()
    {
        var users = new List<User>(); // Initialize a list to store users

        using var connection = _dbContext.GetConnection(); // Get a connection to the database
        connection.Open(); // Open the database connection

        // SQL query to select all users from the Users table
        string query = "SELECT UserId, Username, Email FROM Users";
        using var command = new SQLiteCommand(query, connection); // Prepare the SQL command
        using var reader = command.ExecuteReader(); // Execute the command and get a data reader

        // Read each user record and add it to the users list
        while (reader.Read())
        {
            users.Add(new User
            {
                UserId = Convert.ToInt32(reader["UserId"]), // Convert the user ID to int and store
                Username = reader["Username"].ToString(),   // Store the username
                Email = reader["Email"].ToString()           // Store the email
            });
        }

        // Display users via the view
        _view.DisplayUsers(users); // Call the view method to show the list of users
    }
}
