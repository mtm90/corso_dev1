public class Program
{
    public static void Main()
    {
        TimeSpan timeSpan = new TimeSpan (3, 5, 10, 0);
        // DateTime today = DateTime.Today;
        DateTime today = DateTime.Now;
        DateTime resultDate = today.Add(timeSpan);

        Console.WriteLine("Data e ora risultante: " + resultDate);
    }
}