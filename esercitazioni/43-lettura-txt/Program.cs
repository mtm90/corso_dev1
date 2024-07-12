string path = @".txt";
string[] lines = File.ReadAllLines(path);
foreach (string line in lines)
{
    Console.WriteLine(line);
}