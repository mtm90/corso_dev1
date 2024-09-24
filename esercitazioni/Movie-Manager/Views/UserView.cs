public class UserView
{
    public User GetUserDetailsFromUser()
    {
        // Console input prompts for user details
        Console.Write("Enter username: ");
        string username = Console.ReadLine();

        Console.Write("Enter email: ");
        string email = Console.ReadLine();

        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        // Return a new user object with user input
        return new User
        {
            Username = username,
            Email = email,
            Password = password
        };
    }

    public void ShowUserAddedSuccess(User user)
    {
        Console.WriteLine($"User '{user.Username}' added successfully!");
    }

    public void DisplayUsers(List<User> users)
    {
        Console.WriteLine("\nUsers in the database:");
        foreach (var user in users)
        {
            Console.WriteLine($"{user.UserId}: {user.Username} ({user.Email})");
        }
    }
}
