/*
try 
{
    string contenuto = File.ReadAllText("file.txt");
    Console.WriteLine(contenuto);
}
catch (Exception e)
{
    Console.WriteLine("Il file non esiste");
    Console.WriteLine($"ERRORE NON TRATTATO: {e.Message}");
    return;
}
*/

try 
{
    int[] numeri = new int[int.MaxValue];
}
catch (Exception e)
{
    Console.WriteLine("Memoria insufficiente");
    Console.WriteLine($"ERRORE NON TRATTATO: {e.Message}");
    return;
}
