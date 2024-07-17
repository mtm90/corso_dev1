string path = @"test.csv";
File.Create(path).Close();
while (true)
{
    Console.WriteLine("Inserisci nome, cognome ed età");
    string nome = Console.ReadLine()!;
    string cognome = Console.ReadLine()!;
    string età = Console.ReadLine()!;
    File.AppendAllText(path, nome + "," + cognome + "," + età + "\n");
    string risposta = Console.ReadLine()!;
    if (risposta == "n")
    {
        break;
    }
}