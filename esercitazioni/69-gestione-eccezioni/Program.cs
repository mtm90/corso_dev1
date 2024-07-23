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

int[] numeri = {1,2,3};
try 
{
    Console.WriteLine(numeri[3]);
}
catch (Exception e)
{
    Console.WriteLine("Indice non valido");
    Console.WriteLine($"ERRORE NON TRATTATO: {e.Message}");
    return;
}
finally
{
    Console.WriteLine("Fine del programma");
}