class Program {
     static void Main(string[] args)
     {
        var db = new Database();
        var view = new View(db);
        var controller = new Controller(db, view);
        controller.MainMenu();
     }

}