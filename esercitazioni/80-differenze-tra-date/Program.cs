public class Program
{
    public static void Main()
    {
        DateTime startDate = DateTime.Today;
        DateTime endDate = new DateTime(2024, 1, 1);
        TimeSpan difference = endDate - startDate;
        Console.WriteLine("differenza in giorni: " + difference.Days);
        Console.WriteLine("differenza in ore: " + difference.TotalHours);
        Console.WriteLine("differenza in minuti: " + difference.TotalMinutes);
    }
}