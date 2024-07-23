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
    int zero = 0;
    int numero = 1/ zero;   
}
catch (Exception e)
{
    Console.WriteLine("Divisione per zero");
    Console.WriteLine($"ERRORE NON TRATTATO: {e.Data}");
    Console.WriteLine($"CODICE ERRORE: {e.HResult}");
    return;
}
