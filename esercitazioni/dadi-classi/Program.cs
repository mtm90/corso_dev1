class Dado
{
    private Random random = new Random();
    private int facce;

    public Dado(int facce = 6) 
    {
        this.facce = facce;
    }

    public int Lancia()
    {
        return random.Next(1, facce + 1); 
    }
}

class Program
{
    static void Main(string[] args)
    {
        Dado d1 = new Dado(); 
        Dado d2 = new Dado(12); 

        int n1 = d1.Lancia();
        int n2 = d2.Lancia();

        Console.WriteLine($"Dado 1 (6 facce): {n1}");
        Console.WriteLine($"Dado 2 (12 facce): {n2}");
    }
}
