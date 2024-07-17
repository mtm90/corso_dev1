
List<string> nomi = new List<string>(File.ReadAllLines(@"test.csv"));
string path = @"test.csv";
if (!File.Exists(path))
{
    File.Create(path).Close();
}

while (true)
{
    Console.WriteLine("Inserisci nome, cognome ed età");
    string nome = Console.ReadLine()!;
    string cognome = Console.ReadLine()!;
    string età = Console.ReadLine()!;
    string anagrafica = $"{nome},{cognome},{età}";
    
    if (nomi.Contains(anagrafica))
    {
    Console.WriteLine("The name already exists in the file");
    }
    else
    {
    File.AppendAllText (path, anagrafica + "\n");
    Console.WriteLine("Do you wanna continue? (y/n)");
    string risposta = Console.ReadLine()!;
    if (risposta == "n")
            {
                break;
            }
    }
}


/*
string path = @"test.csv";
if (!File.Exists(path))
{
File.Create(path).Close();
}
while (true)
{
    Console.WriteLine("Inserisci nome, cognome ed età");
    string nome = Console.ReadLine()!;
    string cognome = Console.ReadLine()!;
    string età = Console.ReadLine()!;
    string[] lines = File.ReadAllLines(path);
    bool found = false;
    foreach (string line in lines)
    {
        if (line.StartsWith(nome))
        {
            found = true;
            break;
        }
    }
    if (!found)
    {
        File.AppendAllText(path, nome + "," + cognome + "," + età + "\n");
    }
    else
    {
        Console.WriteLine("Il nome è già presente nel file");
    }
    Console.WriteLine("Do you wanna continue? (y/n)");
    string risposta = Console.ReadLine()!;
    if (risposta == "n")
            {
                break;
            }
    
}


*/
