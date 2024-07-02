Dictionary<string, bool> presenze = new Dictionary<string, bool>();
presenze["Mario Rossi"] = true;
presenze["Luca Bianchi"] = false;

foreach (KeyValuePair<string, bool> studente in presenze)
{
    if (studente.Value)
    {
        Console.WriteLine($"{studente.Key} è presente");
    }
    else
    {
        Console.WriteLine($"{studente.Key} è assente");
    }
}

Console.WriteLine("Di quale studente vuoi cambiare lo stato?");

string nomeStudente = Console.ReadLine()!;

if (presenze.ContainsKey(nomeStudente))
{
    presenze[nomeStudente] = !presenze[nomeStudente];
}
else 
{
    Console.WriteLine("Lo studente non è presente nella lista");
}

foreach (KeyValuePair<string, bool> studente in presenze)
{
    if (studente.Value)
    {
        Console.WriteLine($"{studente.Key} è presente");
    }
    else 
    {
        Console.WriteLine($"{studente.Key} è assente");

    }
}