using Newtonsoft.Json;
class  Program
{
    static void Main(string[] args)
    {
        string path = @"test.json";
        string json = File.ReadAllText(path);
        dynamic obj = JsonConvert.DeserializeObject(json)!;
        string path2 = @"test2.csv";
        File.Create(path2).Close();
        File.AppendAllText(path2, "nome,cognome,eta,via,citta\n");
        for (int i = 0;i < obj.Count; i++)
        {
            File.AppendAllText(path2, $"{obj[i].nome}, {obj[i].cognome}, {obj[i].eta}, {obj[i].indirizzo.via}, {obj[i].indirizzo.citta}\n");
        }
    }
}