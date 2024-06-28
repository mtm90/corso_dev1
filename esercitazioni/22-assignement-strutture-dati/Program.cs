Console.Clear();

/* Versione 1
string[] nomi = new string[8];
nomi[0] = "Mattia";
nomi[1] = "Allison";
nomi[2] = "Ginevra";
nomi[3] = "Daniele";
nomi[4] = "Serghej";
nomi[5] = "Silvano";
nomi[6] = "Matteo";
nomi[7] = "Sharon";

Random random = new Random();
int numero = random.Next(nomi.Length);

Console.WriteLine(nomi[numero]);

*/
/* Versione 2
List<string> nomi = new List<string> {"Mattia", "Allison", "Sharon", "Ginevra", "Daniele", "Matteo", "Silvano", "Serghej"};

Random random = new Random();
int numero = random.Next(nomi.Count);

Console.WriteLine(nomi[numero]);
nomi.RemoveAt(numero);
Console.WriteLine("Elenco nuovi partecipanti:");
for (int i = 0; i < nomi.Count; i++)
{
    Console.WriteLine(nomi[i]);
}
*/

/*
List<string> nomi = new List<string> { "Mattia", "Allison", "Sharon", "Ginevra", "Daniele", "Matteo", "Silvano", "Serghej" };

        Random random = new Random();

        while (nomi.Count > 0)
        {
            int numero = random.Next(nomi.Count);
            Console.WriteLine();
            Console.WriteLine("nome rimosso:");
            Console.WriteLine(nomi[numero]);
            Console.WriteLine();
            nomi.RemoveAt(numero);
            Console.WriteLine("Elenco nuovi partecipanti:");   
                for (int i = 0; i < nomi.Count; i++)
                {
                    Console.WriteLine(nomi[i]);
                }
        }

*/
/* Versione 4
List<string> nomi = new List<string> { "Mattia", "Allison", "Sharon", "Ginevra", "Daniele", "Matteo", "Silvano", "Serghej" };
List<string> nomi2 = new List<string>();

        Random random = new Random();

        while (nomi.Count > 0)
        {
            int indice = random.Next(nomi.Count);
            Console.WriteLine("nome sorteggiato:");
            Console.WriteLine(nomi[indice]);
            Thread.Sleep(300);
            Console.WriteLine();
            nomi2.Add(nomi[indice]);
            nomi.RemoveAt(indice);
            Console.WriteLine("nuovo elenco partecipanti:");   
                foreach (string s in nomi)
                {
                    Console.WriteLine(s);
                    Thread.Sleep(300);
                }
            Console.WriteLine();
            Console.WriteLine("nuova lista:");
                foreach (string s in nomi2)
                {
                    Console.WriteLine(s);
                    Thread.Sleep(300);
                }
            Console.WriteLine();
        }
*/
List<string> nomi = new List<string> { "Mattia", "Allison", "Sharon", "Ginevra", "Daniele", "Matteo", "Silvano", "Serghej" };


bool programIsRunning = true;
while (programIsRunning)
    {
        Console.WriteLine("1. Inserisci partecipante");
        Console.WriteLine("2. Visualizza partecipanti");
        Console.WriteLine("3. Esci");
        int choice = Convert.ToInt32(Console.ReadLine());
        switch (choice)
        {
            case 1:
                string newMember = Console.ReadLine();
                nomi.Add(newMember);
                break;
            case 2:
                foreach (string s in nomi)
                {
                    Console.WriteLine(s);
                } 
                break;
            case 3:
            programIsRunning = false;
            break;
            

        }
    }
    

