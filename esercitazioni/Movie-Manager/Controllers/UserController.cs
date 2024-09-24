public class UserController
{
    private readonly DatabaseContext _dbContext;
    private readonly UserView _view;

    public UserController(DatabaseContext dbContext, UserView view)
    {
        _dbContext = dbContext;
        _view = view;
    }

    public void AddUser()
    {
        // Call the view to get user details from the user
        User newUser = _view.GetUserDetailsFromUser();

        using var connection = _dbContext.GetConnection();
        connection.Open();

        string query = "INSERT INTO Users (Username, Email, Password) VALUES (@Username, @Email, @Password)";
        using var command = new System.Data.SQLite.SQLiteCommand(query, connection);
        command.Parameters.AddWithValue("@Username", newUser.Username);
        command.Parameters.AddWithValue("@Email", newUser.Email);
        command.Parameters.AddWithValue("@Password", newUser.Password); // Hashing recommended

        command.ExecuteNonQuery();

        // Notify the view that the user was added successfully
        _view.ShowUserAddedSuccess(newUser);
    }

    public void ListAllUsers()
    {
        var users = new List<User>();

        using var connection = _dbContext.GetConnection();
        connection.Open();

        string query = "SELECT UserId, Username, Email FROM Users";
        using var command = new System.Data.SQLite.SQLiteCommand(query, connection);
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

        // Display all users using the view
        _view.DisplayUsers(users);
    }
}
