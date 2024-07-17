using Newtonsoft.Json;
class  Program
{
    static void Main(string[] args)
    {
        string path = @"test.csv";
        string[] lines = File.ReadAllLines(path);
        string[][] prodotti = new string[lines.Length][];
        for (int i = 0;i < lines.Length;i++)
        {
            prodotti[i] = lines[i].Split(',');
        }
        for (int i =1; i < prodotti.Length; i++)
        {
            string path2 = prodotti[i][0] + ".json";
            File.Create(path2).Close();
            File.AppendAllText(path2, JsonConvert.SerializeObject(new {nome = prodotti[i][0], prezzo = prodotti[i][1]}));
        }        
    }
}