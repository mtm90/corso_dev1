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
