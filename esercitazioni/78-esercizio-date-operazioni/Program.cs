public class Program
{
    public static void Main()
    {
        DateTime today = DateTime.Today;
        DateTime futureDate = today.AddDays(100);
        DateTime pastDate = today.AddDays(-75); 

        Console.WriteLine("100 giorni da oggi: " + futureDate.ToShortDateString());
        Console.WriteLine("75 giorni prima di oggi: " + pastDate.ToShortDateString());
        
        DateTime nextBirthday = new DateTime(today.Year, 5, 25);
        if (nextBirthday < today)
        {
            nextBirthday = nextBirthday.AddYears(1);
        }
        int daysUntilBirthday = (nextBirthday - today).Days;
        Console.WriteLine("Giorni fino al prossimo compleanno: " + daysUntilBirthday);

    }
}