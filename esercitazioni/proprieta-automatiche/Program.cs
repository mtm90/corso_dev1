/*
public class Persona
{
    public string Nome {get; set;}
    public string Cognome {get; set;}
    public int Eta {get; set;}
}

class Program 
{
    static void Main( string[] args)
    {
        Persona persona = new Persona
        {
            Nome = "Mario",
            Cognome = "Rossi",
            Eta = 30
        };

        Console.WriteLine($"Nome: {persona.Nome}");
        Console.WriteLine($"Cognome: {persona.Cognome}");
        Console.WriteLine($"Eta: {persona.Eta}");

        Console.ReadLine();
    }
}
*/


public class Persona
{
    public string Nome {get; set;}
    public string Cognome {get; set;}
    public int Eta {get; set;}
}
public class Studente : Persona
{
    public string Matricola {get; set;}
    public string CorsoDiStudi {get; set;}
}
class Program 
{
    static void Main( string[] args)
    {
        Studente studente = new Studente
        {
            Nome = "Mario",
            Cognome = "Rossi",
            Eta = 30,
            Matricola = "S123456",
            CorsoDiStudi = "Ingegneria Informatica"

        };

        Console.WriteLine($"Nome: {studente.Nome}");
        Console.WriteLine($"Cognome: {studente.Cognome}");
        Console.WriteLine($"Eta: {studente.Eta}");
        Console.WriteLine($"Matricola: {studente.Matricola}");
        Console.WriteLine($"Eta: {studente.CorsoDiStudi}");

        Console.ReadLine();
    }
}
