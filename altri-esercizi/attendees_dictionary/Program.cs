Dictionary<string, bool> attendees = new Dictionary<string, bool> ();
bool isAttending = true;
attendees.Add("John Doe", isAttending);
attendees.Add("John Wick", !isAttending);
int input;

do
{
    Console.Clear();
    Console.WriteLine("1. Add a new Attendee");
    Console.WriteLine("2. Change an attendee's status");
    Console.WriteLine("3. Remove an attendee");
    Console.WriteLine("4. Display final list");
    Console.WriteLine("5. Quit");
    input = Convert.ToInt32(Console.ReadLine());
switch (input)
{
    case 1:
    Console.WriteLine("Enter the name of the new attendee");
    string name = Console.ReadLine()!;
    Console.WriteLine("Is this person attending? (true/false)");
    string answer = Console.ReadLine()!;
    if (answer == "true")
    {
        attendees.Add(name, isAttending);
    }
    else 
    {
        attendees.Add(name, !isAttending);
    }
    Console.WriteLine("Attendee added succesgully");
    break;
    case 2:
    Console.WriteLine("Enter the name of the attendee to change its status");
    string nameStatus = Console.ReadLine()!;
    if (attendees.ContainsKey(nameStatus))
    {
        if (attendees[nameStatus] == isAttending)
        {
            attendees[nameStatus] = !isAttending;
        }
        else 
        {
            attendees[nameStatus] = isAttending;
        }
        Console.WriteLine("Status changed succesfully");
    }
    else
    {
        Console.WriteLine("The name is not on the list");
    }
    break;
    case 3:
    Console.WriteLine("Enter the name of the attendee u wanna remove");
    string nameToDelete = Console.ReadLine()!;
    if (attendees.ContainsKey(nameToDelete))
    {
        attendees.Remove(nameToDelete);
    }
    else
    {
        Console.WriteLine("The name is not on the list");
    }
    break;
    case 4:
    Console.WriteLine("here are the attendees");
    foreach (KeyValuePair<string, bool> member in attendees)
    {
       if (member.Value == isAttending)
       {
        Console.WriteLine($"{member.Key} is attending");
       }
       else 
       {
        Console.WriteLine($"{member.Key} is not attending");
       }
    }
    break;
    case 5:
    Console.WriteLine("Quitting program");
    break;



    
    
}
}
while (input != 5);