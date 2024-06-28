string[] nomi = new string[8];
nomi[0] = "Mattia";
nomi[1] = "Allison";
nomi[2] = "Ginevra";
nomi[3] = "Daniele";
nomi[4] = "Sergey";
nomi[5] = "Silvano";
nomi[6] = "Matteo";
nomi[7] = "Sharon";

/*
for (int i = 0; i < 8; i++) 
{
    Console.WriteLine(nomi[i]);
}

foreach (string s in nomi)
{
    Console.WriteLine(s);
}
*/
/*
int n = 0;
while (n < 8)
{
    Console.WriteLine(nomi[n]);
    n++;
}
*/
int x = 0;
do
{
    Console.WriteLine(nomi[x]);
    x++;
} while (x < 8);