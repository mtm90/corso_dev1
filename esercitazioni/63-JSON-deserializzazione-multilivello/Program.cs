using Newtonsoft.Json;
class  Program
{
    static void Main(string[] args)
    {
        string path = @"test.json";
        string json = File.ReadAllText(path);
        dynamic obj = JsonConvert.DeserializeObject(json)!;
        Console.WriteLine($"nome:{obj.nome} \ncognome:{obj.cognome} \netà:{obj.eta}"); // stampa il livello 1
        Console.WriteLine($"via: {obj.indirizzo.via} \ncitta: {obj.indirizzo.citta}\n");  // stampa il livello 2
    }
}