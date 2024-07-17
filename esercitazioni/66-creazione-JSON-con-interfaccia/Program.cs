using Newtonsoft.Json;

class Program
{
    static void Main(string[] args)
    {
        string path = @"test.json"; 
        File.Create(path).Close();
        File.AppendAllText(path, "[\n"); // scrive la riga nel file
        

        while (true)
        {
            Console.WriteLine("Inserisci nome e prezzo");
            string nome = Console.ReadLine()!.Trim(); // legge il nome
            string prezzo = Console.ReadLine()!.Trim(); // legge il prezzo

            
                string jsonString = JsonConvert.SerializeObject(new { nome, prezzo }, Formatting.Indented);
                File.AppendAllText(path, jsonString + ",\n");
            /*
            File.AppendAllText(path, JsonConvert.SerializeObject(new {nome, prezzo = prezzo.ToString() }) + ",\n"); // scrive la riga nel file
            */
            Console.WriteLine("Vuoi inserire un altro prodotto? (s/n)");
            string risposta = Console.ReadLine()!;
            if (risposta.Equals("n"))
            {
                break;
            }
        }
        
        // togli l'ultima virgola
        string file = File.ReadAllText(path);
        file = file.Remove(file.Length - 2, 1); // gli argomenti -2 e 1 sono rispettivamente la posizione e il numero di caratteri da rimuovere dalla stringa

        File.WriteAllText(path, file);
        File.AppendAllText(path, "]"); // scrive la riga nel file
    }
}