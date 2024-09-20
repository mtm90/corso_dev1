using System.Data.SQLite;
class Database
{
    private SQLiteConnection _connection;

    public Database()
{
    _connection = new SQLiteConnection("Data Source=database.db");
    _connection.Open();
    
    // Modify the table creation statement to include the 'active' column
    var command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS users (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, active BOOLEAN)", _connection);
    command.ExecuteNonQuery();
}


    public void AddUser(string name, bool active)
    {
        /*
        var command = new SQLiteCommand($"INSERT INTO users (name) VALUES ('{name}')", _connection);
        command.ExecuteNonQuery();
        */
        var command = new SQLiteCommand($"INSERT INTO users (name, active) VALUES (@name, @active)", _connection);
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@active", active);
        command.ExecuteNonQuery();


    }

    public List<User> GetUsers()
    {
        var command = new SQLiteCommand("SELECT id, name, active FROM users", _connection);
        var reader = command.ExecuteReader();
        var users = new List<User>();
        while (reader.Read())
        {
            var id = reader.GetInt32(0); // Retrieve the id
            var name = reader.GetString(1); // Retrieve the name
            var active = reader.GetBoolean(2);
            users.Add(new User(id, name, active)); // Create a new User object
        }
        return users;  
    }

    public void UpdateUser(string oldName, string newName)
    {
        /*
        var command = new SQLiteCommand($"UPDATE users SET name = '{newName}' WHERE name = '{oldName}'", _connection);
        command.ExecuteNonQuery();
        */
        var command = new SQLiteCommand("UPDATE users SET name = @newName WHERE name = @oldName", _connection);
        command.Parameters.AddWithValue("@newName", newName);
        command.Parameters.AddWithValue("@oldName", oldName);
        command.ExecuteNonQuery();
    }

    public void DeleteUser(string name)
    {
        /*
        var command = new SQLiteCommand($"DELETE FROM users WHERE name = '{name}'", _connection);
        command.ExecuteNonQuery();
        */
        var command = new SQLiteCommand($"DELETE FROM users WHERE name = @name", _connection);
        command.Parameters.AddWithValue("@name", name);
        command.ExecuteNonQuery();

    }
    public User SearchUserByName(string name)
{
    var command = new SQLiteCommand("SELECT id, name, active FROM users WHERE name = @name", _connection);
    command.Parameters.AddWithValue("@name", name);
    var reader = command.ExecuteReader();
    
    if (reader.Read())  // If at least one result is found
    {
        var id = reader.GetInt32(0);
        var foundName = reader.GetString(1);
        var active = reader.GetBoolean(2);  // Retrieve active status
        
        return new User(id, foundName, active);  // Return a User object if found
    }
    else
    {
        return null;  // No user found
    }
}

    public void CloseConnection()
    {
        if (_connection.State != System.Data.ConnectionState.Closed)
        {
            _connection.Close();
        }
    }

}
