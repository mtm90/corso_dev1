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
    // Adds a new user to the database
public void AddUser()
{
    // Prompt the user to input their details and retrieve this information through the UserView interface.
    // This method ensures that we collect necessary details for creating a new user, including the username, email, and password.
    User newUser = _view.GetUserDetailsFromUser(); // Calls the UserView method that prompts the user for their details and creates a User object.

    // Establish a connection to the SQLite database using the DatabaseContext.
    // This connection is essential for executing SQL commands and interacting with the database.
    using var connection = _dbContext.GetConnection(); // Obtain a database connection.
    connection.Open(); // Open the connection to the database, which allows subsequent SQL commands to be executed.

    // Prepare the SQL query to insert a new user record into the Users table.
    // The query uses parameterized statements to ensure that the input is treated as data, not executable SQL code.
    string query = "INSERT INTO Users (Username, Email, Password) VALUES (@Username, @Email, @Password)";
    
    // Create a SQLiteCommand object to execute the SQL query, associating it with the active connection.
    using var command = new SQLiteCommand(query, connection); // Prepare the SQL command for execution.

    // Bind the user input parameters to the SQL command to prevent SQL injection attacks.
    // This step enhances security by ensuring that user inputs are safely included in the SQL statement.
    command.Parameters.AddWithValue("@Username", newUser.Username); // Bind the username parameter.
    command.Parameters.AddWithValue("@Email", newUser.Email);       // Bind the email parameter.
    command.Parameters.AddWithValue("@Password", newUser.Password); // Bind the password parameter (Note: Consider using hashing for passwords to enhance security).

    // Execute the SQL command to insert the new user record into the database.
    // The ExecuteNonQuery method is used here as it is designed for commands that do not return results (like INSERT).
    command.ExecuteNonQuery(); // Execute the insert command.

    // Notify the UserView that the user has been successfully added to the database.
    // This step provides feedback to the user interface, indicating that the operation was completed successfully.
    _view.ShowUserAddedSuccess(newUser); // Call the method in UserView to display a success message, confirming the user addition.
}


    // Lists all users from the database
public void ListAllUsers()
{
    // Initialize a list to store users retrieved from the database.
    // This list will hold User objects that are created from the data fetched.
    var users = new List<User>(); // Prepare a new list to store the user records.

    // Obtain a connection to the SQLite database using the DatabaseContext.
    // This connection will allow us to execute SQL commands and interact with the database.
    using var connection = _dbContext.GetConnection(); // Get a connection to the database.
    connection.Open(); // Open the database connection to facilitate data retrieval.

    // Prepare the SQL query to select all user records from the Users table.
    // The query specifies the columns to retrieve, including UserId, Username, and Email.
    string query = "SELECT UserId, Username, Email FROM Users"; 
    using var command = new SQLiteCommand(query, connection); // Create a SQLiteCommand object to execute the query.

    // Execute the command and obtain a data reader to read the results.
    // The ExecuteReader method is used because we expect multiple records as a result of the query.
    using var reader = command.ExecuteReader(); // Execute the command and get a data reader to read the results.

    // Loop through the data reader to read each user record from the database.
    // The Read method advances the reader to the next record, returning true if there are more records.
    while (reader.Read())
    {
        // Create a new User object for each record and populate it with data from the reader.
        // Convert the UserId to an integer, and read the Username and Email as strings.
        users.Add(new User
        {
            UserId = Convert.ToInt32(reader["UserId"]), // Convert the user ID to int and store in the User object.
            Username = reader["Username"].ToString(),   // Read the username and store in the User object.
            Email = reader["Email"].ToString()           // Read the email and store in the User object.
        });
    }

    // Once all users have been read and stored in the list, we notify the view to display the users.
    // This involves calling a method in the UserView to present the user data to the interface.
    _view.DisplayUsers(users); // Call the method in UserView to display the list of users retrieved from the database.
}

}
