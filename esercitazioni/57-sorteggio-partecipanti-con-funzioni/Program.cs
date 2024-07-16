
class Program
{
    static List<string> members = new List<string> { "Mattia", "Allison", "Unknown", "Ginevra", "Daniele", "Matteo", "Francesco", "Serghej" };
    const string path = @"members.txt";

    static void Main()
    {
        LoadMembers();
        int input;
        do
        {
            Console.Clear();
            Console.WriteLine("1. Insert member");
            Console.WriteLine("2. show members");
            Console.WriteLine("3. order members");
            Console.WriteLine("4. find member");
            Console.WriteLine("5. delete member");
            Console.WriteLine("6. edit member");
            Console.WriteLine("7. sort in teams");
            Console.WriteLine("8. sort in teams with GetRange");
            Console.WriteLine("9. Quit");
            Console.WriteLine("Choice:");
            input = Convert.ToInt32(Console.ReadLine());
            switch(input)
            {
                case 1:
                AddMember();
                break;
                case 2:
                ShowMembers();
                break;
                case 3:
                OrderMembers();
                break;
                case 4:
                FindMembers();
                break;
                case 5:
                DeleteMember();
                break;
                case 6:
                EditMember();
                break;
                case 7:
                SortInTeams();
                break;
                case 8:
                SortInTeamsWithGetRange();
                break;
                case 9:
                break;
            }
        }
        while (input != 9);

    }
    static void SaveMembers()
    {
        File.WriteAllLines(path, members);

    }
    static void AddMember()
    {
        Console.Clear();
            Console.WriteLine("Enter new member name");
            string newMember = Console.ReadLine()!;
            if (members.Contains(newMember))
            {
                Console.WriteLine($"{newMember} is already in the list, try a different one");
                Thread.Sleep(1000);
            }
            else
            {
                members.Add(newMember);
                File.AppendAllText(path, newMember + "\n");
                Console.WriteLine($"{newMember} was added to the Members Menu");
                Thread.Sleep(1000);
            }
    }
    static void ShowMembers()
    {
        Console.Clear();
            Console.WriteLine("Members:");
            Console.WriteLine("");
            foreach (var member in members)
            {
                Console.WriteLine(member);
            }
            Console.WriteLine($"Total members: {members.Count}");
            Console.WriteLine();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey(true);
    }
    static void OrderMembers()
    {
        Console.Clear();
            Console.WriteLine("1. Alphabetical order");
            Console.WriteLine("2. Alphabetical order inverted");
            Console.WriteLine("3. Quit");
            int sortInput = Convert.ToInt32(Console.ReadLine());

            switch (sortInput)
            {
                case 1:
                    members.Sort();
                    Console.WriteLine("Memmbers order changed");
                    Thread.Sleep(1000);

                    break;
                case 2:
                    members.Sort();
                    members.Reverse();
                    Console.WriteLine("Memmbers order changed");
                    Thread.Sleep(1000);
                    break;
                case 3:
                    break;
            }
    }
    static void FindMembers()
    {
        Console.WriteLine("Insert name:");
            string name = Console.ReadLine()!;
            if (members.Contains(name))
            {
                Console.WriteLine("The name is listed");
                Thread.Sleep(1000);

            }
            else
            {
                Console.WriteLine("The name is not listed");
                Thread.Sleep(1000);
            }
    }
    static void DeleteMember()
    {
        Console.WriteLine("Type in the name of the member you want to delete");
            string memberToDelete = Console.ReadLine()!;
            if (members.Contains(memberToDelete))
            {
                members.Remove(memberToDelete);
                Console.WriteLine("The name was deleted");
                Thread.Sleep(1000);
                SaveMembers();
            }
            else
            {
                Console.WriteLine("The name is not listed");
                Thread.Sleep(1000);

            }
    }
    static void EditMember()
    {
        Console.WriteLine("Member name:");
            string name = Console.ReadLine()!;
            if (members.Contains(name))
            {
                Console.WriteLine("New name: ");
                string newName = Console.ReadLine()!;
                int index = members.IndexOf(name);
                members[index] = newName;
                Console.WriteLine("The member was successfully edited");
                Thread.Sleep(1000);
                SaveMembers();
            }
            else
            {
                Console.WriteLine("The member is not in the list");
                Thread.Sleep(1000);
            }
    }
    static void SortInTeams()
    {
        Console.Clear();
        Random mix = new();
        bool addToteam1 = true;
        List<string> team1 = [];
        List<string> team2 = [];
        while (members.Count > 0)
        {
            int randomIndex = mix.Next(0, members.Count);
            if (addToteam1)
            {
                team1.Add(members[randomIndex]);
                addToteam1 = false;
            }
            else
            {
                team2.Add(members[randomIndex]);
                addToteam1 = true;
            }
            members.RemoveAt(randomIndex);
        }
        Console.WriteLine("team1:");
        Console.WriteLine("");
        foreach ( string member in team1)
        {
            Console.WriteLine(member);
            Thread.Sleep(1000);
        }
        Console.WriteLine("");
        Console.WriteLine("team2:");
        Console.WriteLine("");
        foreach ( string member in team2)
        {
            Console.WriteLine(member);
            Thread.Sleep(1000);
        }
        Console.WriteLine("");
        members.AddRange(File.ReadAllLines(path));

    }
    static void SortInTeamsWithGetRange()
    {
        int split = members.Count/2;
        List<string> squadra1 = members.GetRange(0, split);    
        List<string> squadra2 = members.GetRange(split, members.Count - split);
        Console.WriteLine("squadra 1:");
        foreach (string member in squadra1)
        {
            Console.WriteLine(member);
            Thread.Sleep(1000);
        }    
        Console.WriteLine("squadra 2:");
        foreach (string member in squadra2)
        {
            Console.WriteLine(member);
            Thread.Sleep(1000);
        }  
    }
    static void LoadMembers()
    {
        if (File.Exists(path) && File.ReadAllText(path).Length != 0)
        {
            string[] savedMembers = File.ReadAllLines(path);
            members = new List<string>(savedMembers);
        }
        else
        {
            // If the file does not exist or is empty, write the initial members to the file
            SaveMembers();
        }
    }
}
