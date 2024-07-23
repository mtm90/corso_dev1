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
    int numero = int.Parse("1000000000000");   // genera una eccezzione perchè il numero è troppo grosso
}
catch (Exception e)
{
    Console.WriteLine("Il numero è troppo alto");
    Console.WriteLine($"ERRORE NON TRATTATO: {e.Message}");
    Console.WriteLine($"CODICE ERRORE: {e.HResult}");
    return;
}
