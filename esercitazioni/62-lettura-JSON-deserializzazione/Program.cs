using Newtonsoft.Json;
class  Program
{
    static void Main(string[] args)
    {
        string path = @"test.json";
        string json = File.ReadAllText(path);
        dynamic obj = JsonConvert.DeserializeObject(json)!;
        Console.WriteLine($"nome:{obj.nome} \ncognome:{obj.cognome} \netà:{obj.eta} \n");
    }
}