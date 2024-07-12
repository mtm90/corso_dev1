string path = @"nomi.txt";
string [] lines = File.ReadAllLines(path);
string [] nomi = new string [lines.Length];
for (int i = 0;i < lines.Length; i++)
{
    nomi[i] = lines[i];
}
bool noNameStartsWithA = true;
foreach (string nome in nomi)
{
    if (nome.ToLower().StartsWith("a"))
    {
        Console.WriteLine(nome);
        noNameStartsWithA = false;
    }
}
if (noNameStartsWithA)
    {
        Console.WriteLine("nessun nome inizia con A");
    }