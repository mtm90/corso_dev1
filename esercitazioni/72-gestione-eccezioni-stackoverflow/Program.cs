class Program
{
    static void Main(string[] args)
    {
        try
        {
            StackOverflow(); // il metodo StackOverflow() viene chiamato ricorsivamente perciò la pila si riempie e il programma si blocca
        }
        catch (Exception e)
        {
            Console.WriteLine("StackOverflow");
            Console.WriteLine($"ERRORE NON TRATTATO: {e.Message}");
            return;
        }
    }
    static void StackOverflow()
    {
        StackOverflow();
    }
}