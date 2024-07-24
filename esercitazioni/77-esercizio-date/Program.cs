public class Program
{
    public static void Main()
    {
        DateTime birthDate = new DateTime(1990, 5, 25);
        Console.WriteLine("Formato lungo: " + birthDate.ToLongDateString());
        Console.WriteLine("Mese in formato testuale: " + birthDate.ToString("MMMM"));
        Console.WriteLine("Formato personalizzato: " + birthDate.ToString("dd-MM-yyyy"));

    }
}