// Handles operations related to users such as adding and listing users
using System.Data.SQLite;
public class UserController
{
    private readonly DatabaseContext _dbContext;
    private readonly UserView _view;

    public UserController(DatabaseContext dbContext, UserView view)
    {
        _dbContext = dbContext;
        _view = view;
    }

    // Adds a new user to the database
    public void AddUser()
    {
        // Get user details from the view
        User newUser = _view.GetUserDetailsFromUser();

        using var connection = _dbContext.GetConnection();
        connection.Open();

        string query = "INSERT INTO Users (Username, Email, Password) VALUES (@Username, @Email, @Password)";
        using var command = new SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@Username", newUser.Username);
        command.Parameters.AddWithValue("@Email", newUser.Email);
        command.Parameters.AddWithValue("@Password", newUser.Password);  // Password hashing recommended
        command.ExecuteNonQuery();

        // Notify view of successful user addition
        _view.ShowUserAddedSuccess(newUser);
    }

    // Lists all users from the database
    public void ListAllUsers()
    {
        var users = new List<User>();

        using var connection = _dbContext.GetConnection();
        connection.Open();

        string query = "SELECT UserId, Username, Email FROM Users";
        using var command = new SQLiteCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            users.Add(new User
            {
                UserId = Convert.ToInt32(reader["UserId"]),
                Username = reader["Username"].ToString(),
                Email = reader["Email"].ToString()
            });
        }

        // Display users via the view
        _view.DisplayUsers(users);
    }
}
