﻿string path = @"test.txt";
string[] lines = File.ReadAllLines(path);
string[] nomi = new string[lines.Length];
for (int i = 0; i < lines.Length; i++)
{
    nomi[i] = lines[i];
}
Random random = new Random();
int index = random.Next(nomi.Length);
Console.WriteLine(nomi[index]);