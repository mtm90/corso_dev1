int input;
Dictionary<string,int> shoppingList = new Dictionary<string, int>();
do {

Console.Clear();
Console.WriteLine("Welcome to your shopping List!");
Console.WriteLine("1. Add an item");
Console.WriteLine("2. remove an item");
Console.WriteLine("3. display the list");
Console.WriteLine("4. quit");

input = Convert.ToInt32(Console.ReadLine());

switch(input) {
    case 1:
    Console.WriteLine("Enter the name of the item");
    string itemName = Console.ReadLine()!;
    Console.WriteLine("Enter the quantity");
    int itemQnt = Convert.ToInt32(Console.ReadLine());

    shoppingList.Add(itemName, itemQnt);

    Console.WriteLine("Item added succesfully");
    Thread.Sleep(1500);
    break;
    case 2:
        Console.WriteLine("Enter the name of the item u wanna remove");
        string itemToRemove = Console.ReadLine()!;
        shoppingList.Remove(itemToRemove);
        Console.WriteLine("Item removed succesfully");
        Thread.Sleep(1500);
        break;

    case 3:
    foreach (KeyValuePair<string, int> article in shoppingList)
    {
        Console.WriteLine($"article: {article.Key}, quantity: {article.Value}");
    }
    Thread.Sleep(1500);
    break;
    case 4:
    Console.WriteLine("Quitting");
    Thread.Sleep(1500);
    break;
}
}
while (input != 4);