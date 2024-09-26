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
    public bool IsActive { get; set; } = true;
}

class Subscription
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}


/*
Components Explained
Primary Key:
TransactionId: This property serves as the primary key for the Transaction entity. By default, EF Core looks for a property named Id or <ClassName>Id (like TransactionId) to identify the primary key.

Foreign Keys:
UserId: This integer property is intended to reference the primary key of the User entity. By naming it UserId, EF Core infers that this is a foreign key pointing to the User table.
SubscriptionId: Similarly, this property references the primary key of the Subscription entity, and its naming convention allows EF Core to infer its role as a foreign key.

Navigation Properties:
User: This property is of type User and acts as a navigation property. It allows you to access the related User entity from a Transaction instance.
Subscription: This property is of type Subscription and serves the same purpose for the Subscription entity.
*/
class Transaction
{
    public int TransactionId { get; set; } // Primary Key

    public int UserId { get; set; } // This property is inferred as a foreign key to the User entity
    public User User { get; set; } // Navigation property that refers to the User entity

    public int SubscriptionId { get; set; } // This property is inferred as a foreign key to the Subscription entity
    public Subscription Subscription { get; set; } // Navigation property that refers to the Subscription entity

    public DateTime StartDate { get; set; } // A property that stores the start date of the subscription
}



class Database : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

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
        Console.WriteLine("1. User Management");
        Console.WriteLine("2. Subscription Management");
        Console.WriteLine("3. Transactions Management");
        Console.WriteLine("4. Exit");
    }


    public void ShowSubscriptionMenu()
    {
        Console.WriteLine("1. Add Subscription");
        Console.WriteLine("2. View Subscriptions");
        Console.WriteLine("3. Update Subscription");
        Console.WriteLine("4. Delete Subscription");
        Console.WriteLine("5. Back to Main Menu");
    }

    public void ShowUserMenu()
    {
        Console.WriteLine("1. Add User");
        Console.WriteLine("2. View Users");
        Console.WriteLine("3. Update User");
        Console.WriteLine("4. Delete User");
        Console.WriteLine("5. Back to Main Menu");
    }

    public void ShowTransactionMenu()
    {
        Console.WriteLine("1. Add Transaction");
        Console.WriteLine("2. Show Transactions");
        Console.WriteLine("3. Delete Transaction");
        Console.WriteLine("4. Update Transaction");
        Console.WriteLine("5. Back to Main Menu");

    }


    public void ShowUsers(List<User> users)
    {
        foreach (var user in users)
        {
            Console.WriteLine($"ID: {user.Id} Name: {user.Name} Is Active: {user.IsActive}");
        }
    }

    public void ShowSubscriptions(List<Subscription> subscriptions)
    {

        foreach (var subscription in subscriptions)
        {
            Console.WriteLine($"ID: {subscription.Id} Name: {subscription.Name} Price: {subscription.Price:C}");
        }
    }

    public void ShowTransactions(List<Transaction> transactions)
    {
        foreach (var transaction in transactions)
        {
            Console.WriteLine($" Transaction ID: {transaction.TransactionId} User ID: {transaction.UserId} SubscriptionID: {transaction.SubscriptionId} Start Date: {transaction.StartDate}");
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
                UserMenu();
            }
            else if (input == "2")
            {
                SubscriptionMenu();
            }
            else if (input == "3")
            {
                TransactionMenu();
            }
            else if (input == "4")
            {
                break;
            }
        }
    }


    private void AddUser()
    {
        Console.WriteLine("Enter username:");
        var name = _view.GetInput();

        Console.WriteLine("Is the user active? (yes/no):");
        var activeInput = _view.GetInput().ToLower();

        bool isActive = activeInput == "yes";

        _db.Users.Add(new User { Name = name, IsActive = isActive });
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
            if (u.Name == oldName)
            {
                user = u;
                break;
            }
        }

        if (user != null)
        {
            user.Name = newName;
            _db.SaveChanges();
        }

        Console.WriteLine("Do you want to change the active status? (yes/no):");
        var activeInput = _view.GetInput().ToLower();

        bool isActive = activeInput == "yes";

        user.Name = newName;
        user.IsActive = isActive;

        _db.SaveChanges();

        Console.WriteLine("User updated successfully!");
    }

    private void DeleteUser()
    {
        Console.WriteLine("Enter username u want to delete");
        var name = _view.GetInput();

        User userToDelete = null;
        foreach (var user in _db.Users)
        {
            if (user.Name == name)
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

    private void AddSubscription()
    {
        Console.WriteLine("Enter Subscription Name:");
        var subscriptionName = Console.ReadLine();

        Console.WriteLine("Enter Subscription Price:");
        decimal.TryParse(Console.ReadLine(), out decimal price);
        _db.Subscriptions.Add(new Subscription { Name = subscriptionName, Price = price });
        _db.SaveChanges();
        Console.WriteLine("Subscription added successfully!");
    }

    private void ShowSubscriptions()
    {
        var subscriptions = _db.Subscriptions.ToList();
        _view.ShowSubscriptions(subscriptions);
    }

    private void UpdateSubscription()
    {
        Console.WriteLine("Enter the old name of the subscription to update:");
        var oldName = _view.GetInput();
        Console.WriteLine("Enter the new name of the subscription");
        var newName = _view.GetInput();
        Console.WriteLine("Enter the new price of the subscription");
        var newPrice = Convert.ToDecimal(_view.GetInput());

        Subscription sub = null;

        foreach (var s in _db.Subscriptions)
        {
            if (s.Name == oldName)
            {
                sub = s;
                break;
            }
        }

        if (sub != null)
        {
            sub.Name = newName;
            sub.Price = newPrice;
            _db.SaveChanges();
        }
    }

    private void DeleteSubscription()
    {
        Console.WriteLine("Enter the name of the subscription to delete:");
        var name = _view.GetInput();

        Subscription subToDelete = null;
        foreach (var sub in _db.Subscriptions)
        {
            if (sub.Name == name)
            {
                subToDelete = sub;
                break;
            }
        }
        if (subToDelete != null)
        {
            _db.Subscriptions.Remove(subToDelete);
            _db.SaveChanges();
        }

    }

    private void SubscriptionMenu()
    {
        while (true)
        {
            _view.ShowSubscriptionMenu();
            var input = _view.GetInput();

            if (input == "1")
            {
                AddSubscription();
            }
            else if (input == "2")
            {
                ShowSubscriptions();
            }
            else if (input == "3")
            {
                UpdateSubscription();
            }
            else if (input == "4")
            {
                DeleteSubscription();
            }
            else if (input == "5")
            {
                break; // Return to the main menu
            }
        }
    }

    private void UserMenu()
    {
        while (true)
        {
            _view.ShowUserMenu();
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
                break; // Return to the main menu
            }
        }
    }

    private void TransactionMenu()
    {
        while (true)
        {
            _view.ShowTransactionMenu();
            var input = _view.GetInput();

            if (input == "1")
            {
                AddTransaction();
            }
            else if (input == "2")
            {
                ShowTransactions();
            }
            else if (input == "3")
            {
                DeleteTransaction();
            }
            else if (input == "4")
            {
                UpdateTransaction();
            }
            else if (input == "5")
            {
                break; // Return to the main menu
            }
        }
    }

    private void AddTransaction()
    {
        Console.WriteLine("Enter User ID:");
        var userIdInput = _view.GetInput();
        int.TryParse(userIdInput, out int userId);

        // Get Subscription ID
        Console.WriteLine("Enter Subscription ID:");
        var subscriptionIdInput = _view.GetInput();
        int.TryParse(subscriptionIdInput, out int subscriptionId);

        var transaction = new Transaction
        {
            UserId = userId,
            SubscriptionId = subscriptionId,
            StartDate = DateTime.Now // Set the StartDate as the current date/time
        };

        _db.Transactions.Add(transaction);
        _db.SaveChanges();
        Console.WriteLine("Transaction added successfully!");
    }





    private void ShowTransactions()
    {
        var transactions = _db.Transactions.ToList();
        _view.ShowTransactions(transactions);
    }

    private void DeleteTransaction()
    {
        Console.WriteLine("Enter the ID of the transaction u want to delete:");
        int id = Convert.ToInt32(_view.GetInput());

        Transaction tranId = null;
        foreach (var tran in _db.Transactions)
        {
            if (tran.TransactionId == id)
            {
                tranId = tran;
            }
        }
        if (tranId != null)
        {
            _db.Transactions.Remove(tranId);
            _db.SaveChanges();
            Console.WriteLine("Transaction succesfully deleted:");
        }
    }

    private void UpdateTransaction()
{
    Console.WriteLine("Enter the ID of the transaction to update:");
    if (int.TryParse(_view.GetInput(), out int transactionId))
    {
        // Find the transaction in the database
        Transaction transaction = null;

        foreach (var t in _db.Transactions)
        {
            if (t.TransactionId == transactionId)
            {
                transaction = t;
                break;
            }
        }

        if (transaction != null)
        {
            Console.WriteLine("Enter the new start date for the transaction (yyyy-mm-dd):");
            if (DateTime.TryParse(_view.GetInput(), out DateTime newStartDate))
            {
                transaction.StartDate = newStartDate; // Update the start date
                _db.SaveChanges(); // Save changes to the database
                Console.WriteLine("Transaction updated successfully.");
            }
            else
            {
                Console.WriteLine("Invalid date format. Transaction not updated.");
            }
        }
        else
        {
            Console.WriteLine("Transaction not found.");
        }
    }
    else
    {
        Console.WriteLine("Invalid Transaction ID.");
    }
}







}