﻿Console.Clear();

/* Versione 1
string[] nomi = new string[8];
nomi[0] = "Mattia";
nomi[1] = "Allison";
nomi[2] = "Ginevra";
nomi[3] = "Daniele";
nomi[4] = "Serghej";
nomi[5] = "Silvano";
nomi[6] = "Matteo";
nomi[7] = "Sharon";

Random random = new Random();
int numero = random.Next(nomi.Length);

Console.WriteLine(nomi[numero]);

*/
/* Versione 2
List<string> nomi = new List<string> {"Mattia", "Allison", "Sharon", "Ginevra", "Daniele", "Matteo", "Silvano", "Serghej"};

Random random = new Random();
int numero = random.Next(nomi.Count);

Console.WriteLine(nomi[numero]);
nomi.RemoveAt(numero);
Console.WriteLine("Elenco nuovi partecipanti:");
for (int i = 0; i < nomi.Count; i++)
{
    Console.WriteLine(nomi[i]);
}
*/

/*
List<string> nomi = new List<string> { "Mattia", "Allison", "Sharon", "Ginevra", "Daniele", "Matteo", "Silvano", "Serghej" };

        Random random = new Random();

        while (nomi.Count > 0)
        {
            int numero = random.Next(nomi.Count);
            Console.WriteLine();
            Console.WriteLine("nome rimosso:");
            Console.WriteLine(nomi[numero]);
            Console.WriteLine();
            nomi.RemoveAt(numero);
            Console.WriteLine("Elenco nuovi partecipanti:");   
                for (int i = 0; i < nomi.Count; i++)
                {
                    Console.WriteLine(nomi[i]);
                }
        }

*/
/* Versione 4
List<string> nomi = new List<string> { "Mattia", "Allison", "Sharon", "Ginevra", "Daniele", "Matteo", "Silvano", "Serghej" };
List<string> nomi2 = new List<string>();

        Random random = new Random();

        while (nomi.Count > 0)
        {
            int indice = random.Next(nomi.Count);
            Console.WriteLine("nome sorteggiato:");
            Console.WriteLine(nomi[indice]);
            Thread.Sleep(300);
            Console.WriteLine();
            nomi2.Add(nomi[indice]);
            nomi.RemoveAt(indice);
            Console.WriteLine("nuovo elenco partecipanti:");   
                foreach (string s in nomi)
                {
                    Console.WriteLine(s);
                    Thread.Sleep(300);
                }
            Console.WriteLine();
            Console.WriteLine("nuova lista:");
                foreach (string s in nomi2)
                {
                    Console.WriteLine(s);
                    Thread.Sleep(300);
                }
            Console.WriteLine();
        }
*/
List<string> members = new List<string> { "Mattia", "Allison", "Silvano", "Ginevra", "Daniele", "Matteo", "Francesco", "Serghej" };

bool programIsRunning = true; // creo una variabile booleana che definisce quando il programma deve funzionare
bool sortMenuOn = false;

while (programIsRunning)
    {
        Console.WriteLine("Menu:");
        Console.WriteLine("1. Insert member");
        Console.WriteLine("2. show members");
        Console.WriteLine("3. order members");
        Console.WriteLine("4. find member");
        Console.WriteLine("5. delete member");
        Console.WriteLine("6. edit member");
        Console.WriteLine("7. sort in teams");
        Console.WriteLine("8. sort in teams with GetRange");
        Console.WriteLine("9. Quit");

        int choice = Convert.ToInt32(Console.ReadLine()); 
            switch (choice) 
            {
                case 1:
                    Console.Clear();
                    string newMember = Console.ReadLine();
                    if (members.Contains(newMember))
                    {
                        Console.WriteLine("The name is already in the list, try a different one");
                    }
                    else
                    {
                        members.Add(newMember);
                    }               
                    break;
                case 2:
                    Console.Clear();
                    foreach (string s in members)
                    {Console.WriteLine(s);
                    Thread.Sleep(300);
                    } 
                    Console.WriteLine($"Total members: {members.Count}");
                    break;
                case 3:
                    Console.Clear();
                    do
                    {
                    Console.WriteLine("Choose an option:");
                    Console.WriteLine("1. Alphabetical order");
                    Console.WriteLine("2. Alphabetical order inverted");
                    int choice2 = Convert.ToInt32(Console.ReadLine());
                    switch (choice2)
                        {
                            case 1:
                            members.Sort();
                            sortMenuOn = false;
                            break;
                            case 2:
                            members.Sort();
                            members.Reverse();
                            sortMenuOn = false;
                            break;
                            default:
                            Console.WriteLine("Try again:");
                            sortMenuOn = true;
                            break;
                        }
                    }
                    while (sortMenuOn);
                break;        
                case 4:
                    Console.WriteLine("insert name:");
                    string name = Console.ReadLine();
                    if (members.Contains(name))
                    {
                        Console.WriteLine("The name is listed");
                    }
                    else 
                    {
                        Console.WriteLine("The name is not listed");
                    }
                    break;
                case 5:
                    Console.WriteLine("Type in the name of the member you want to delete");
                    string memberToDelete = Console.ReadLine();
                    if (members.Contains(memberToDelete))
                    {
                        members.Remove(memberToDelete);
                        Console.WriteLine("The name was deleted");
                    }
                    else {
                        Console.WriteLine("The name is not listed");
                    }
                    break;
                case 6:
                    Console.WriteLine("member name:");
                    name = Console.ReadLine();
                    if (members.Contains(name))
                    {
                        Console.WriteLine("New name: ");
                        string newName = Console.ReadLine();
                        int index = members.IndexOf(name);
                        members[index] = newName;
                        Console.WriteLine("the member was successfully edited");
                    }
                    else 
                    { 
                        Console.WriteLine("the member is not in list");
                    }
                break;
                case 7:
                    List<string> team1 = [];
                    List<string> team2 = [];
                    Random mix = new();
                    bool addToteam1 = true;
                    while (members.Count > 0)
                    {
                        int randomIndex = mix.Next(0, members.Count);
                        if(addToteam1)
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
                    break;
                case 8:
                    int split = members.Count/2;
                    List<string> squadra1 = members.GetRange(0, split);    
                    List<string> squadra2 = members.GetRange(split, members.Count - split);
                    Console.WriteLine("squadra 1:");
                    foreach (string member in squadra1)
                    {
                        Console.WriteLine(member);
                    }    
                    Console.WriteLine("squadra 2:");
                    foreach (string member in squadra2)
                    {
                        Console.WriteLine(member);
                    }  
                    break;
                case 9:
                    programIsRunning = false;
                    break;
                default:
                    Console.WriteLine("Try Again:");
                    break;
            }
    }


