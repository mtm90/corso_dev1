class Controller
{
    private Database _db;
    private View _view;

    public Controller(Database db, View view)
    {
        _db = db;
        _view = view;
    }

    public void MainMenu()
    {
        while (true)
        {
            _view.ShowMainMenu();
            var input = _view.GetInput();
            if (input == "1")
            {
                AddUser();
            }
            else if (input == "2")
            {
                ShowUser();
            }
            else if (input == "3")
            {
                UpdateUser();
            }
            else if (input == "4")
            {
                DeleteUser();
            }
            else if (input == "5")
            {
                SearchUserByName();
            }
            else if (input == "6")
            {
                _db.CloseConnection();
                break;
            }
        }
    }

    private void DeleteUser()
    {
        Console.WriteLine("Enter user name you want to delete:");
        var name = _view.GetInput();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Please insert valid input");
            DeleteUser();
        }
        else 
        {
            _db.DeleteUser(name);
        }
    }

    private void UpdateUser()
    {
        Console.WriteLine("Enter user name you want to update:");
        var oldName = _view.GetInput();
        if (string.IsNullOrWhiteSpace(oldName))
        {
            Console.WriteLine("Please insert valid input");
            UpdateUser();
        }
        else 
        {
        Console.WriteLine("Enter new username:");
        var newName = _view.GetInput();
        _db.UpdateUser(oldName, newName);
        }

    }

    private void AddUser()
    {
        Console.WriteLine("Enter user name:");
        var name = _view.GetInput();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Please insert valid input");
            AddUser();
        }
        else 
        {
            Console.WriteLine("Is the user active? (y/n)");
        var answer = _view.GetInput();
        bool active = answer.ToLower() == "y";  // Set active to true if the input is 'y', otherwise false

        _db.AddUser(name, active);  // Pass both name and active status to the AddUser method
    }
        
    }

    private void ShowUser()
    {
        var users = _db.GetUsers();
        _view.ShowUsers(users);
    }

    private void SearchUserByName()
{
    Console.WriteLine("Enter the name you want to search:");
    var name = _view.GetInput();  
    
    var user = _db.SearchUserByName(name);  // Call the database method
    
    if (user != null)
    {
        _view.ShowUsers(new List<User> { user });  // Display the found user
    }
    else
    {
        Console.WriteLine("User not found.");  // Notify that no user was found
    }
}


}
