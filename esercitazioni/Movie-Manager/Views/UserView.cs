using Spectre.Console;

public class UserView
{
    public User GetUserDetailsFromUser()
    {
        string username = AnsiConsole.Ask<string>("Enter [green]username[/]:");
        string email = AnsiConsole.Ask<string>("Enter [green]email[/]:");
        string password = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter [green]password[/]:")
                .PromptStyle("red")
                .Secret());

        return new User
        {
            Username = username,
            Email = email,
            Password = password
        };
    }

    public void ShowUserAddedSuccess(User user)
    {
        AnsiConsole.Markup($"[green]User '{user.Username}'[/] added successfully!");
    }

    public void DisplayUsers(List<User> users)
    {
        var table = new Table();
        table.AddColumn("User ID");
        table.AddColumn("Username");
        table.AddColumn("Email");

        foreach (var user in users)
        {
            table.AddRow(
                user.UserId.ToString(),
                user.Username,
                user.Email
            );
        }

        AnsiConsole.Write(table);
    }
}
