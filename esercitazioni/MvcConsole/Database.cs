using System.Data.SQLite;
class Database
{
    private SQLiteConnection _connection;

    public Database()
    {
        _connection = new SQLiteConnection("Data Source=database.db");
        _connection.Open(); 
        var command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS users (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT)", _connection);
        command.ExecuteNonQuery();  
    }

    public void AddUser(string name)
    {
        var command = new SQLiteCommand($"INSERT INTO users (name) VALUES ('{name}')", _connection);
        command.ExecuteNonQuery();
    }

    public List<User> GetUsers()
    {
        var command = new SQLiteCommand("SELECT id, name FROM users", _connection);
        var reader = command.ExecuteReader();
        var users = new List<User>();
        while (reader.Read())
        {
            var id = reader.GetInt32(0); // Retrieve the id
            var name = reader.GetString(1); // Retrieve the name
            users.Add(new User(id, name)); // Create a new User object
        }
        return users;  
    }

    public void UpdateUser(string oldName, string newName)
    {
        var command = new SQLiteCommand($"UPDATE users SET name = '{newName}' WHERE name = '{oldName}'", _connection);
        command.ExecuteNonQuery();
    }

    public void DeleteUser(string name)
    {
        var command = new SQLiteCommand($"DELETE FROM users WHERE name = '{name}'", _connection);
        command.ExecuteNonQuery();
    }
    public User SearchUserByName(string name)
{
    var command = new SQLiteCommand($"SELECT id, name FROM users WHERE name = '{name}'", _connection);
    var reader = command.ExecuteReader();
    
    if (reader.Read())  // If at least one result is found
    {
        var id = reader.GetInt32(0);
        var foundName = reader.GetString(1);
        return new User(id, foundName);  // Return a User object if found
    }
    else
    {
        return null;  // No user found
    }
}

}
