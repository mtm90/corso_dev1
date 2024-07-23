class Program
{
    static void Main(string[] args)
    {
        try
        {
            int numero = int.Parse("ciao"); // il metodo int.Parse() genera un'eccezione perché "ciao" non è un numero
        }
        catch (Exception e)
        {
            Console.WriteLine("Il numero non è valido");
            Console.WriteLine($"ERRORE NON TRATTATO: {e.Message}");
            return;
        }
    }
}