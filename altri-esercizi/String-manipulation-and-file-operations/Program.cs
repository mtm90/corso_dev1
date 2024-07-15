string path = @"names.txt";
File.Create(path).Close();
string[] names = ["Paolo", "Giulio", "Andrea"];
foreach (string nome in names)
{
    File.AppendAllText(path, nome + "\n");
}
string[] lines = File.ReadAllLines(path);
string path2 = @"uppercase_names.txt";
File.Create(path2).Close();
foreach (string line in lines)
{
    File.AppendAllText(path2, line.ToUpper() + "\n");
}
string path3 = @"name_lengths";
File.Create(path3).Close();
foreach (string line in lines)
{
    File.AppendAllText(path3, line + ":" + line.Length + "\n");
}