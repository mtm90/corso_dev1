using Spectre.Console; // Namespace for enhanced console applications

// View class responsible for managing user interactions related to user accounts
public class UserView
{
    // Collects user details (username, email, password) from the user
    public User GetUserDetailsFromUser()
    {
        // Prompt the user to enter their username
        string username = AnsiConsole.Ask<string>("Enter [green]username[/]:");
        // Prompt the user to enter their email
        string email = AnsiConsole.Ask<string>("Enter [green]email[/]:");
        // Prompt the user to enter their password securely
        string password = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter [green]password[/]:")
                .PromptStyle("red") // Change prompt style to red for emphasis
                .Secret() // Hide user input for security
        );

        // Return a new User object with the collected details
        return new User
        {
            Username = username,
            Email = email,
            Password = password
        };
    }

    // Displays a success message for a newly added user
    public void ShowUserAddedSuccess(User user)
    {
        // Display success message with the username
        AnsiConsole.Markup($"[green]User '{user.Username}'[/] added successfully!");
    }

    // Displays all users in a table format
    public void DisplayUsers(List<User> users)
    {
        var table = new Table(); // Create a new table instance
        table.AddColumn("User ID"); // Add column for User ID
        table.AddColumn("Username"); // Add column for Username
        table.AddColumn("Email"); // Add column for Email

        // Add each user's data to the table
        foreach (var user in users)
        {
            table.AddRow(
                user.UserId.ToString(), // Convert User ID to string
                user.Username,          // Username
                user.Email              // Email
            );
        }

        // Display the constructed table in the console
        AnsiConsole.Write(table);
    }
}
