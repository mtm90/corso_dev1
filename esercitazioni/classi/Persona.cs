/* Primo Esempio
class Persona
{
    public string nome;
    public string cognome;
    public int eta;

    public Persona(string nome, string cognome, int eta) // i campi vengono inizializzati
    {
        this.nome = nome;
        this.cognome = cognome;
        this.eta = eta;

    }
    public void Stampa()
    {
        Console.WriteLine($"Nome: {nome}");
        Console.WriteLine($"Cognome: {cognome}");
        Console.WriteLine($"Età: {eta}");
    }
}

*/


/* Secondo Esempio
class Persona
{
    private string nome;
    private string cognome;
    private int eta;

    public string Nome

    {
        get { return nome;}
        set { nome = value;}
    }
    public string Cognome

    {
        get { return cognome;}
        set { cognome = value;}
    }
    public int Eta

    {
        get { return eta;}
        set { eta = value;}
    }

    public Persona(string nome, string cognome, int eta)
    {
        this.nome = nome;
        this.cognome = cognome;
        this.eta = eta;
    }

    public void Stampa()
    {
        Console.WriteLine($"Nome: {nome}");
        Console.WriteLine($"Cognome: {cognome}");
        Console.WriteLine($"Età: {eta}");    }

}
*/

class Persona 
{
    private string nome;
    private string cognome;
    private int eta;

    public Persona(string nome, string cognome, int eta)
    {
        this.nome = nome;
        this.cognome = cognome;
        this.eta = eta;
    }  
    public void Stampa()
    {
        Console.WriteLine($"Nome: {nome}");
        Console.WriteLine($"Cognome: {cognome}");
        Console.WriteLine($"Età: {eta}");
    }  
}

class Studente : Persona 
{
    public string corso;
    public Studente (string nome, string cognome, int eta, string corso) : base(nome, cognome, eta)
    {
        this.corso = corso;
    }
    public void StampaCorso()
    {
        Console.WriteLine($"Corso: {corso}");

    }
}


