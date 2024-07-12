string path = @"test.txt";
string[] lines = File.ReadAllLines(path);
string[] nomi = new string[lines.Length];
for (int i = 0; i < lines.Length; i++)
{
    nomi[i] = lines[i];
}
Random random = new Random();
int index = random.Next(nomi.Length);
Console.WriteLine(nomi[index]);
string path2 = @"test2.txt";
File.Create(path2).Close();
File.AppendAllText(path2, nomi[index] + "\n");