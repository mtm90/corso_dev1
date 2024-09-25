using Spectre.Console; // Namespace for enhanced console applications

// View class responsible for managing user interactions related to user accounts
public class UserView
{
    // Collects user details (username, email, password) from the user
    public User GetUserDetailsFromUser()
    {
        // Prompt the user to enter their username.
        // This will be the unique identifier for the user in the system.
        string username = AnsiConsole.Ask<string>("Enter [green]username[/]:"); // Get username
        
        // Prompt the user to enter their email address.
        // The email is essential for account verification and communication.
        string email = AnsiConsole.Ask<string>("Enter [green]email[/]:"); // Get email
        
        // Prompt the user to enter their password securely.
        // Password input is masked for security reasons to prevent others from seeing it.
        string password = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter [green]password[/]:")
                .PromptStyle("red") // Change prompt style to red for emphasis and visibility
                .Secret() // Hide user input for enhanced security
        );

        // Return a new User object populated with the collected details.
        // This encapsulates user data in an object-oriented manner for better management and validation.
        return new User
        {
            Username = username, // Assign the entered username to the User object
            Email = email,       // Assign the entered email to the User object
            Password = password   // Assign the entered password to the User object
        };
    }

    // Displays a success message for a newly added user
    public void ShowUserAddedSuccess(User user)
    {
        // Display a success message indicating that the user has been added successfully.
        // The message highlights the username, giving feedback to the user about their action.
        AnsiConsole.Markup($"[green]User '{user.Username}'[/] added successfully!");
        
        // Optionally, you could add a prompt for the user to continue or perform further actions.
        // AnsiConsole.Markup($"Press [green]Enter[/] to continue");
        // Console.ReadLine(); // Wait for user input (if needed)
    }

    // Displays all users in a table format
    public void DisplayUsers(List<User> users)
    {
        // Create a new table instance to visually present user data in an organized manner.
        var table = new Table(); // Create a new table instance
        
        // Define the columns for the table, which represent various attributes of each user.
        table.AddColumn("User ID");   // Add a column for User ID
        table.AddColumn("Username");   // Add a column for Username
        table.AddColumn("Email");      // Add a column for Email

        // Iterate over each user in the provided list of users.
        // This allows for populating the table with all user records from the database or application state.
        foreach (var user in users)
        {
            // Add a new row to the table for each user, containing their details.
            // Each piece of user information is converted to a string for display purposes.
            table.AddRow(
                user.UserId.ToString(), // Convert User ID to string for display
                user.Username,          // Display Username
                user.Email              // Display Email
            );
        }

        // Finally, display the constructed table in the console.
        // This renders the table with all user information for the user to view.
        AnsiConsole.Write(table); // Display the constructed table in the console
    }
}
