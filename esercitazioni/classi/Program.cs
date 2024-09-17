/* Primo esempio
class Program
{
    static void Main(string[] args)
    {
        Persona p = new Persona("Mario", "Rossi", 30);
        p.Stampa();
    }
}
*/
/* Secondo esempio
class Program
{
    static void Main(string[] args)
    {
        Persona p = new Persona("Mario", "Rossi", 30);
        p.Stampa();
        p.Nome = "Luigi";
        p.Stampa();
    }
}
*/ 

class Program 
{
    static void Main(string[] args)
    {
        Studente s = new Studente("Mario", "Rossi", 30, "Informatica");
        s.Stampa();
        s.StampaCorso();
    }
}