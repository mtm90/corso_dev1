class Program
{
    static void Main(string[] args)
    {
        var db = new Database(); // Initialize the database
        var view = new View();   // Initialize the view (UI)
        var controller = new Controller(db, view); // Initialize the controller (logic)

        controller.MainMenu(); // Start the application
    }
}
