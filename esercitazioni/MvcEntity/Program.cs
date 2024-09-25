using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;

class Program
{
    static void Main(string[] args)
    {
        var db = new Database(); // Model
        var view = new View(db); // View
        var controller = new Controller(db, view); // Controller
        controller.MainMenu();
    }
}

class User
{
    public int Id { get; set; }
    public string Name { get; set; }
}

class Database : DbContext
{
    public DbSet<User> Users {get;set;}

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=database.db");
    }
}

class View 
{
    private Database _db;

    public View(Database db)
    {
        _db = db;
    }
    public void ShowMainMenu()
    {
        Console.WriteLine("1. Aggiungi user");
        Console.WriteLine("2. Leggi users");
        Console.WriteLine("3. Modifica user");
        Console.WriteLine("4. Elimina user");
        Console.WriteLine("5. Esci");
    }

    public void ShowUsers(List<User> users)
    {
        foreach (var user in users)
        {
            Console.WriteLine(user.Name);
        }
    }

    public string GetInput()
    {
        return Console.ReadLine();
    }
}

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
                ShowUsers();
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
                break;
            }
        }
    }

    private void AddUser()
    {
        Console.WriteLine("Enter username");
        var name = _view.GetInput();
        _db.Users.Add(new User { Name = name });
        _db.SaveChanges();
    }

    private void ShowUsers()
    {
        var users = _db.Users.ToList();
        _view.ShowUsers(users);
    }

    private void UpdateUser()
    {
        Console.WriteLine("Enter old username");
        var oldName = _view.GetInput();
        Console.WriteLine("Enter new username");
        var newName = _view.GetInput();

        User user = null;

        foreach (var u in _db.Users)
        {
            if ( u.Name == oldName)
            {
                user = u;
                break;
            }
        }

        if ( user != null)
        {
            user.Name = newName;
            _db.SaveChanges();
        }
    }

    private void DeleteUser()
    {
        Console.WriteLine("Enter username u want to delete");
        var name = _view.GetInput();

        User userToDelete = null;
        foreach ( var user in _db.Users)
        {
            if ( user.Name == name)
            {
                userToDelete = user;
                break;
            }
        }
        if (userToDelete != null)
        {
            _db.Users.Remove(userToDelete);
            _db.SaveChanges();
        }
    }



}