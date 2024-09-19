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
        Console.WriteLine("5. Cerca user");
        Console.WriteLine("6. Esci");
    }

    public void ShowUsers(List<User> users)
    {
        foreach (var user in users)
        {
            Console.WriteLine($"ID: {user.Id}, name: {user.Name}"); // Display both ID and name
        }
    }

    public string GetInput()
    {
        return Console.ReadLine();
    }

    public string UpdateUser()
    {
        return Console.ReadLine();
    }

    public string DeleteUser()
    {
        return Console.ReadLine();
    }

    public string SearchUserByName()
    {
        return Console.ReadLine();
    }
}
