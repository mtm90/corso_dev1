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
    int numero = int.Parse(null);   
}
catch (Exception e)
{
    Console.WriteLine("Il numero non può essere Null");
    Console.WriteLine($"ERRORE NON TRATTATO: {e.Message}");
    return;
}
finally
{
    Console.WriteLine("Fine del programma");
}