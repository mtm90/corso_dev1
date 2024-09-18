using Newtonsoft.Json;

public class Persona 
{
    public string Nome {get; set;}
    public int Eta {get; set;}
    public bool Impiegato {get; set;}
}

public class GestioneJson
{
    public static void Main(string[] args)
    {
        Persona persona = new Persona
        {
            Nome = "Mario Rossi",
            Eta = 20,
            Impiegato = true
        };

            string json = JsonConvert.SerializeObject(persona, Formatting.Indented );
    File.WriteAllText(@"persona.json", json);

    string jsonDaLeggere = File.ReadAllText(@"persona.json");
    Persona personaDeserializzata = JsonConvert.DeserializeObject<Persona>(jsonDaLeggere)!;

    Console.WriteLine(personaDeserializzata!.Nome);
    }

}