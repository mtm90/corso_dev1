using Newtonsoft.Json;

// Legge tutti i file JSON nella cartella del programma e permette all'utente di selezionarne uno per visualizzarne il contenuto

string cartella = @"C:\Users\dev1\Documents\corso_dev1\dev1_2024\esercitazioni\85-JSON-lettura-file-dentro-cartella";

string[] files = Directory.GetFiles(cartella, "*.json");

if (files.Length == 0)
{
    Console.WriteLine("Non ci sono file JSON nelle cartelle");
    return;
}

Console.WriteLine("Elenco dei file JSON");
for (int i = 0; i < files.Length; i++)
{
    Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}");
}

Console.WriteLine("Quali file vuoi leggere? (Inserisci il numero corrispondente):");
if (int.TryParse(Console.ReadLine(), out int scelta) && scelta > 0 && scelta <= files.Length) // tenta di convertire l'input in un numero intero e verifica che sia compreso tra 1 e il numero di file

{
    string fileScelto = files[scelta -1];
    string json = File.ReadAllText(fileScelto);
    dynamic obj = JsonConvert.DeserializeObject(json)!;
    Console.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented));
}
else 
{
    Console.WriteLine("Scelta non valida");
}